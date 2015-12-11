/// <binding />
"use strict"
var gulp = require('gulp');
var less = require('gulp-less');
var minify = require('gulp-minify');
var rename = require('gulp-rename');

gulp.task('LessCSS', function () {
    gulp.src('Content/Site.less')
    .pipe(less())
    .pipe(gulp.dest('Content'));
});

gulp.task('Minify', function () {
    gulp.src('js/home-index.js')
    .pipe(minify())
    //.pipe(rename({suffix: '.min'}))
    .pipe(gulp.dest('js'));
});