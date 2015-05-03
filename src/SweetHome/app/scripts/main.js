define(['require'], function(require) {
	'use strict';
	var $ = require(['jquery']);
	var helper = require(['second']);
	var three = require(['third']);
	three.one();
	require(['bxslider']);
	$('.main').bxSlider();
	console.log(helper.name);
});
