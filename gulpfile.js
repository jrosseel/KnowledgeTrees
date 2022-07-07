var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    size = require('gulp-size');

gulp.task('front_app', function () {
   return gulp.src('frontend/src/**/*.js')
      .pipe(uglify())
      .pipe(concat('app.js'))
      .pipe(size())
      .pipe(gulp.dest('frontend/dist'));
});


//Other modules: .pipe(csso())