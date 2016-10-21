"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream"),
    del = require("del"),
    bundleconfig = require("./bundleconfig.json"),
    path = require("path"),
    less = require("gulp-less");

var webroot = "./wwwroot/";
var webrootlib = "./wwwroot/lib/";

gulp.task("min", ["min:js", "min:css"]);

gulp.task("min:js", function () {
    var tasks = getBundles(".js").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("less", function () {
    return gulp.src("./wwwroot/content/less/**/*.less")
      .pipe(less({
          paths: [path.join(__dirname, "less", "includes")]
      }))
      .pipe(gulp.dest("./wwwroot/content/css"));
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

gulp.task("build", ["clean", "min:js", "min:css"]);

gulp.task("watch", ["build"], function () {
    getBundles(".js").forEach(function (bundle) {
        bundle.inputFiles.forEach(function (inputFile){
            gulp.watch(inputFile, ["min:js"]);
        })
    });

    gulp.watch("./wwwroot/content/js/*.js", ["min:js"]);
    gulp.watch("./wwwroot/content/less/*.less", ["min:css"]);
});

function getBundles(outputFileNameRegex) {
    return bundleconfig.filter(function (bundle) {
        return new RegExp(outputFileNameRegex).test(bundle.outputFileName);
    });
}