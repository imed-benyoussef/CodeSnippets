﻿module.exports = function(config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine', '@angular-devkit/build-angular'],
        plugins: [
            require('karma-jasmine'),
            require('karma-chrome-launcher'),
            require('karma-jasmine-html-reporter'),
            require('karma-coverage'),
            require('@angular-devkit/build-angular/plugins/karma')
        ],
        client: {
            clearContext: false // leave Jasmine Spec Runner output visible in browser
        },
        coverageReporter: {
            dir: require('path').join(__dirname, './coverage/aiglusoft.iam.client'),
            subdir: '.',
            reporters: [
                { type: 'html' },
                { type: 'text-summary' },
                { type: 'json', file: 'coverage-summary.json' }
            ]
        },
        reporters: ['progress', 'kjhtml', 'coverage'],
        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['ChromeHeadless'],
        singleRun: false,
        restartOnFileChange: true
    });
};