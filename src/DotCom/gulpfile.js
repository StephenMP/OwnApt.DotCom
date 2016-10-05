/// <binding AfterBuild='minify:css' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    clean = require('gulp-rimraf'),
    concat = require('gulp-concat'),
    less = require("gulp-less"),
    livereload = require('gulp-livereload'),
    rename = require('gulp-rename'),
    cssmin = require('gulp-cssmin'),
    runSequence = require('run-sequence');

var webroot = "./wwwroot/";
var webrootlib = "./wwwroot/lib/";

var paths = {
    cssSource: "./Styles/Shared/*.less",
    cssDest: webroot + "styles/shared/",
    css: webroot + "styles/shared/*.css",
    cssBootstrap: webrootlib + "bootstrap/dist/css/bootstrap.css",
    cssFontAwesome: webrootlib + "font-awesome/css/font-awesome.css",
    cssWow: webrootlib + "wow/css/libs/animate.css",
    cssToastr: webrootlib + "toastr/toastr.css",
    concatCssDest: webroot + "css/site.css"
};

var home = {
    css: {
        animate: webroot + "css/animate.min.css",
        creative: webroot + "lib-unmanaged/creative/css/creative.css",
        destination: webroot + "css/home.css"
    }
};

var owner = {
    less: {
        source: "./Styles/Owner/*.less"
    },
    css: {
        destination: webroot + "styles/owner/",
        files: webroot + "styles/owner/*.css",
        concat: webroot + "css/owner.css"
    }
}


gulp.task('clean:css', function (cb) {
    return gulp.src(webroot + "/css/*").pipe(clean());
});

gulp.task('less', [], function () {
    return gulp.src([paths.cssSource])
           .pipe(less())
           .pipe(gulp.dest(paths.cssDest));
});

gulp.task('less:owner', [], function () {
    return gulp.src([owner.less.source])
           .pipe(less())
           .pipe(gulp.dest(owner.css.destination));
});

gulp.task('concat:css', ['less'], function () {
    return gulp.src([paths.css, paths.cssBootstrap, paths.cssFontAwesome, paths.cssWow, paths.cssToastr])
           .pipe(concat(paths.concatCssDest))
           .pipe(gulp.dest("."));
});

gulp.task('concat:owner:css', ['less:owner'], function () {
    return gulp.src([owner.css.files])
           .pipe(concat(owner.css.concat))
           .pipe(gulp.dest("."));
});

gulp.task('concat:home:css', [], function () {
    return gulp.src([home.css.animate, home.css.creative])
           .pipe(concat(home.css.destination))
           .pipe(gulp.dest("."));
});

gulp.task('minify-css', ['concat:css', 'concat:home:css', 'concat:owner:css'], function(){
    return gulp.src([paths.concatCssDest, home.css.destination, owner.css.concat])
           .pipe(cssmin())
           .pipe(rename({ suffix: '.min' }))
           .pipe(gulp.dest(webroot + "css"));
});

gulp.task('build', function (done) {
    runSequence('clean:css',
                'minify-css',
                 done);
});

gulp.task('watch', function () {
    gulp.watch([paths.cssSource,owner.less.source] , ['build']);
});

