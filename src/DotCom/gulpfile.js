"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream"),
    del = require("del"),
    bundleconfig = require("./bundleconfig.json"),
    path = require("path"),
    less = require("gulp-less"),
    runSequence = require('run-sequence');

gulp.task("less", function () {
    return gulp.src("./wwwroot/content/less/**/*.less")
      .pipe(less({
          paths: [path.join(__dirname, "less", "includes")]
      }))
      .pipe(gulp.dest("./wwwroot/content/css"));
});

gulp.task("min:js", function () {
    var tasks = getBundles(".js").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .pipe(gulp.dest("."));
    });

    return merge(tasks);
});

gulp.task("min:css", ["less"], function () {
    var tasks = getBundles(".css").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest("."));
    });

    return merge(tasks);
});

gulp.task("clean", function () {
    var files = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });

    files.push("./wwwroot/content/css/**/*");

    return del(files);
});

gulp.task("build", function () {
    runSequence("clean", ["min:js", "min:css"]);
});

gulp.task("watch", ["build"], function () {
    gulp.watch("./wwwroot/content/**/*.less", ["min:css"]);
    gulp.watch("./wwwroot/content/**/*.js", ["min:js"]);
});

function getBundles(outputFileNameRegex) {
    return bundleconfig.filter(function (bundle) {
        return new RegExp(outputFileNameRegex).test(bundle.outputFileName);
    });
}