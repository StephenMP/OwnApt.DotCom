/// <binding AfterBuild='less' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    rimraf = require('rimraf'),
    concat = require('gulp-concat'),
    less = require("gulp-less"),
    livereload = require('gulp-livereload'),
    uglify = require('gulp-cssmin');

var webroot = "./wwwroot/";
var webrootlib = "./wwwroot/lib/";

var paths = {
    cssSource: "./Styles/**/*.less",
    cssDest: webroot + "styles",
    css: webroot + "styles/**/*.css",
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


gulp.task('clean:css', function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task('clean:home:css', function (cb) {
    rimraf(home.css.destination, cb);
});

gulp.task('less', ['clean:css'], function () {
    return gulp.src([paths.cssSource])
           .pipe(less())
           .pipe(gulp.dest(paths.cssDest));
});

gulp.task('concat:css', ['less'], function () {
    return gulp.src([paths.css, paths.cssBootstrap, paths.cssFontAwesome, paths.cssWow, paths.cssToastr])
           .pipe(concat(paths.concatCssDest))
           .pipe(gulp.dest("."));
});

gulp.task('concat:home:css', ['clean:home:css'], function () {
    return gulp.src([home.css.animate, home.css.creative])
           .pipe(concat(home.css.destination))
           .pipe(gulp.dest("."));
});

gulp.task('minify:css', ['concat:css', 'concat:home:css'], function(){
    return gulp.src([paths.concatcssDest, home.css.destination])
           .pipe(cssmin())
           .pipe(rename({ suffix: '.min' }))
           .pipe(gulp.dest("."));
});

gulp.task('watch', function () {
    gulp.watch(paths.cssSource, ['concat:css']);
});

