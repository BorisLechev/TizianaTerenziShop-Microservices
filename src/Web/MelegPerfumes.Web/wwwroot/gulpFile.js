var gulp = require('gulp'),
    sass = require('grunt-sass');

gulp.task('build-css', function () {
    return gulp
        .src('./scss/**/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('./css'));
});

// Default Task
gulp.task('default', ['build-css']);