using System;

namespace SweetHome.Utils
{
	public static class DateUtils
	{
		public static string SexyYears(int years)
		{
			if (years != 1){
				if (years > 4){
					return "лет";
				}
				return "года";
			}
			return "год";
		}
		public static string SexyMonths(int months)
		{
			if (months > 1){
				if (months > 4){
					return "месяцев";
				}
				return "месяца";
			}
			return "месяц";
			
		}
		
		public static string Age(DateTime birthday,DateTime today)
		{
			int days = (int)(today - birthday).TotalDays;
			string result;
			if (days < 365){
				int months = (int)(days/30);
				
				result = months + " " + SexyMonths(months);
			}
			else{
				int years = (int)(days/365);
				result = years + " " + SexyYears(years);
			}
			return result;
		}
	}
}