define(function(require) {
    'use strict';
	var img_img = function(url) {
        return '<img class="img" src="' + url + '">';
    };
    var img_div = function(url) {
        return '<div class="img" style="background-image: url(\'' + url + '\');"></div>';
    };
    return {
        img: img_img,
        div: img_div
    }
});