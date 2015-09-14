var libPath = "../lib/";

var config = {
    paths: {
        "jquery": libPath + "jquery/dist/jquery",
        "bootstrap": libPath + "bootstrap/js",
        "bxslider": libPath + "bxslider-4/dist/jquery.bxslider"
    },
    shim: {
        'bootstrap/affix':      { deps: ['jquery'], exports: '$.fn.affix' }, 
        'bootstrap/alert':      { deps: ['jquery'], exports: '$.fn.alert' },
        'bootstrap/button':     { deps: ['jquery'], exports: '$.fn.button' },
        'bootstrap/carousel':   { deps: ['jquery'], exports: '$.fn.carousel' },
        'bootstrap/collapse':   { deps: ['jquery'], exports: '$.fn.collapse' },
        'bootstrap/dropdown':   { deps: ['jquery'], exports: '$.fn.dropdown' },
        'bootstrap/modal':      { deps: ['jquery'], exports: '$.fn.modal' },
        'bootstrap/popover':    { deps: ['jquery'], exports: '$.fn.popover' },
        'bootstrap/scrollspy':  { deps: ['jquery'], exports: '$.fn.scrollspy' },
        'bootstrap/tab':        { deps: ['jquery'], exports: '$.fn.tab'        },
        'bootstrap/tooltip':    { deps: ['jquery'], exports: '$.fn.tooltip' },
        'bootstrap/transition': { deps: ['jquery'], exports: '$.fn.transition' },
        'bxslider':             { deps: ['jquery'], exports: '$.fn.bxSlider' }
    }
};

require.config(config);
