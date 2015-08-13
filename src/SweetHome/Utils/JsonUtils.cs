using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SweetHome.Utils
{
    public class CustomJsonTextWriter : JsonTextWriter
    {
        public CustomJsonTextWriter(TextWriter textWriter) : base(textWriter) {}
    
        public int CurrentDepth { get; private set; }
    
        public override void WriteStartObject()
        {
            CurrentDepth++;
            base.WriteStartObject();
        }
    
        public override void WriteEndObject()
        {
            CurrentDepth--;
            base.WriteEndObject();
        }
    }
    
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly Func<bool> _includeProperty;
    
        public CustomContractResolver(Func<bool> includeProperty)
        {
            _includeProperty = includeProperty;
        }
    
        protected override JsonProperty CreateProperty(
            MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = obj => _includeProperty() &&
                                              (shouldSerialize == null ||
                                               shouldSerialize(obj));
            return property;
        }
    }
    
    /// <summary>
    /// Special JsonConvert resolver that allows you to ignore properties.  See http://stackoverflow.com/a/13588192/1037948
    /// </summary>
    public class IgnorableSerializerContractResolver : DefaultContractResolver {
        protected readonly Dictionary<Type, HashSet<string>> Ignores;
    
        public IgnorableSerializerContractResolver() {
            this.Ignores = new Dictionary<Type, HashSet<string>>();
        }
    
        /// <summary>
        /// Explicitly ignore the given property(s) for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName">one or more properties to ignore.  Leave empty to ignore the type entirely.</param>
        public void Ignore(Type type, params string[] propertyName) {
            // start bucket if DNE
            if (!this.Ignores.ContainsKey(type)) this.Ignores[type] = new HashSet<string>();
    
            foreach (var prop in propertyName) {
                this.Ignores[type].Add(prop);
            }
        }
    
        /// <summary>
        /// Is the given property for the given type ignored?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool IsIgnored(Type type, string propertyName) {
            if (!this.Ignores.ContainsKey(type)) return false;
    
            // if no properties provided, ignore the type entirely
            if (this.Ignores[type].Count == 0) return true;
    
            return this.Ignores[type].Contains(propertyName);
        }
    
        /// <summary>
        /// The decision logic goes here
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
    
            if (this.IsIgnored(property.DeclaringType, property.PropertyName)
            // need to check basetype as well for EF -- @per comment by user576838
            || this.IsIgnored(property.DeclaringType.BaseType, property.PropertyName)) {
                property.ShouldSerialize = instance => { return false; };
            }
    
            return property;
        }
        public IgnorableSerializerContractResolver Ignore<TModel>(Expression<Func<TModel, object>> selector)
        {
            MemberExpression body = selector.Body as MemberExpression;
        
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)selector.Body;
                body = ubody.Operand as MemberExpression;
        
                if (body == null)
                {
                    throw new ArgumentException("Could not get property name", "selector");
                }
            }
        
            string propertyName = body.Member.Name;
            this.Ignore(typeof (TModel), propertyName);
            return this;
        }
    }
    
    public static class Json
    {
    	public static string SerializeObject(object obj, int maxDepth)
    	{
    	    using (var strWriter = new StringWriter())
    	    {
    	        using (var jsonWriter = new CustomJsonTextWriter(strWriter))
    	        {
    	            Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
    	            var resolver = new CustomContractResolver(include);
    	            var serializer = new JsonSerializer {ContractResolver = resolver};
    	            serializer.Serialize(jsonWriter, obj);
    	        }
    	        return strWriter.ToString();
    	    }
    	}
        public static string SerializeObject(object obj, IContractResolver contract)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = contract });
        }
        public static string SerializeObject<T>(object obj, Expression<Func<T, object>> selector)
        {
            return SerializeObject(obj, new IgnorableSerializerContractResolver().Ignore<T>(selector));
        }
    }
}
