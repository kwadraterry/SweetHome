var gulp = require('gulp'),
	sourcemaps = require('gulp-sourcemaps'),
    uglify = require('gulp-uglify'),
	rename = require('gulp-rename'),
	_if = require('gulp-if'),
	_ignore = require('gulp-ignore'),
	imagemin = require('gulp-imagemin'),
	mainBowerFiles = require('main-bower-files'),
	del = require('del'),
	project = require('./project.json'),
	lessPluginInlineUrl = require('less-plugin-inline-urls'),
	dst = project.webroot,
	src = project.approot,
	paths = {
		src: {
			scripts: src + '/scripts',
			styles: src + '/styles',
			bower: src + '/lib',
			images: src + '/images'
		},
		dst: {
			scripts: dst + '/js',
			styles: dst + '/css',
			bower: dst + '/lib',
			images: dst + '/images'
		}
	};

gulp.task('default', function () {
	
});

gulp.task('styles', ['fixBxSlider', 'clean:css'], function() {
	var less = require('gulp-less'),
		autoprefixer = require('gulp-autoprefixer'),
		csso = require('gulp-csso');

	return gulp.src([paths.src.styles + '/**/*.less', paths.src.styles + '/**/*.css'])
		.pipe(sourcemaps.init())
		.pipe(_if('**/*.less', less({
			relativeUrls: true
		})))
		.pipe(autoprefixer())
		.pipe(csso())
		.pipe(sourcemaps.write('.'))
		.pipe(gulp.dest(paths.dst.styles));
});

gulp.task('scripts', ['almond'], function() {
	var amdOptimize = require('amd-optimize'),
		concat = require('gulp-concat');

	return gulp.src([
			paths.src.scripts + '/**/*.js',
			paths.src.bower + '/**/*.js',
			'!**/_references.js'])
		.pipe(sourcemaps.init())
		.pipe(amdOptimize("main", {
			findNestedDependencies: true,
    		baseUrl: paths.src.scripts,
			configFile: paths.src.scripts + "/require_config.js"
		}))
		.pipe(uglify())
		.pipe(concat("index.js"))
		.pipe(sourcemaps.write('.'))
		.pipe(gulp.dest(paths.dst.scripts));
});

gulp.task('almond', ['clean:js'], function () {
    return gulp.src(paths.src.bower + '/almond/almond.js')
        .pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(paths.dst.scripts));
});

gulp.task('images', ['clean:img'], function() {
	return gulp.src(paths.src.images + '/*.*')
		.pipe(imagemin())
		.pipe(gulp.dest(paths.dst.images));
});

gulp.task('bower:copy', ['clean:lib'], function() {
	return gulp.src(mainBowerFiles(), {base: paths.src.bower})
		// no need to serve source files: they will be bundled in one js and one css
		.pipe(_ignore.exclude(["**/*.js", "**/*.css", "**/*.less"]))
		.pipe(imagemin()) // ignore non-images, compress images from libs
		.pipe(gulp.dest(paths.dst.bower));
});

gulp.task('watch', function() {
    gulp.watch(paths.src.images + '/*.*', ['images']);
    gulp.watch(paths.src.scripts + '/**/*.js', ['scripts']);
	gulp.watch([paths.src.styles + '/**/*.less', paths.src.styles + '/**/*.css'], ['styles']);
});

gulp.task('clean:js', function (done) {
	return del(paths.dst.scripts, done);
});

gulp.task('clean:css', function (done) {
	return del(paths.dst.styles, done);
});

gulp.task('clean:img', function (done) {
	return del(paths.dst.images, done);
});

gulp.task('clean:lib', function(done) {
	return del(paths.dst.bower, done);
});

gulp.task('clean:all', function(done) {
	return del(dst, done);
});

gulp.task('fixBxSlider', function() {
	return gulp.src(paths.src.bower + '/bxslider-4/src/less/*.less')
		.pipe(gulp.dest(paths.src.bower + '/bxslider-4/src'));
});
