/// <binding AfterBuild='less' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    rimraf = require('rimraf'),
    concat = require('gulp-concat'),
    less = require("gulp-less");

var webroot = "./wwwroot/";

var paths = {
    cssSource: "./Styles/**/*.less",
    cssDest: webroot + "styles",
    css: webroot + "styles/**/*.css",
    concatCssDest: webroot + "css/siteless.css"
};


gulp.task('less', function () {
    return gulp.src([paths.cssSource])
           .pipe(less())
           .pipe(gulp.dest(paths.cssDest));
});

gulp.task('clean:css', function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task('concat:css', function () {
    return gulp.src(paths.css)
           .pipe(concat(paths.concatCssDest))
           .pipe(gulp.dest("."));
});

gulp.task('compile:css', ['clean:css', 'less', 'concat:css'])
