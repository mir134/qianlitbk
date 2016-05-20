/*!
 * headroom.js v0.3.11 - Give your page some headroom. Hide your header until you need it
 * Copyright (c) 2013 Nick Williams - http://wicky.nillia.ms/headroom.js
 * License: MIT
 */

(function (global) {

    'use strict';

    window.requestAnimationFrame = window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame;

    /**
     * Handles debouncing of events via requestAnimationFrame
     * @see http://www.html5rocks.com/en/tutorials/speed/animations/
     * @param {Function} callback The callback to handle whichever event
     */
    function Debouncer(callback) {
        this.callback = callback;
        this.ticking = false;
    }
    Debouncer.prototype = {
        constructor: Debouncer,

        /**
         * dispatches the event to the supplied callback
         * @private
         */
        update: function () {
            this.callback && this.callback();
            this.ticking = false;
        },

        /**
         * ensures events don't get stacked
         * @private
         */
        requestTick: function () {
            if (!this.ticking) {
                requestAnimationFrame(this.update.bind(this));
                this.ticking = true;
            }
        },

        /**
         * Attach this as the event listeners
         */
        handleEvent: function () {
            this.requestTick();
        }
    };
    /**
     * UI enhancement for fixed headers.
     * Hides header when scrolling down
     * Shows header when scrolling up
     * @constructor
     * @param {DOMElement} elem the header element
     * @param {Object} options options for the widget
     */
    function Headroom(elem, options) {
        options = options || Headroom.options;

        this.lastKnownScrollY = 0;
        this.elem = elem;
        this.debouncer = new Debouncer(this.update.bind(this));
        this.tolerance = options.tolerance;
        this.classes = options.classes;
        this.offset = options.offset;
        this.initialised = false;
    }
    Headroom.prototype = {
        constructor: Headroom,

        /**
         * Initialises the widget
         */
        init: function () {
            this.elem.classList.add(this.classes.initial);

            // defer event registration to handle browser 
            // potentially restoring previous scroll position
            setTimeout(this.attachEvent.bind(this), 100);
        },

        /**
         * Unattaches events and removes any classes that were added
         */
        destroy: function () {
            this.initialised = false;
            window.removeEventListener('scroll', this.debouncer, false);
            this.elem.classList.remove(this.classes.unpinned, this.classes.pinned, this.classes.initial);
        },

        /**
         * Attaches the scroll event
         * @private
         */
        attachEvent: function () {
            if (!this.initialised) {
                this.initialised = true;
                window.addEventListener('scroll', this.debouncer, false);
            }
        },

        /**
         * Unpins the header if it's currently pinned
         */
        unpin: function () {
            this.elem.classList.add(this.classes.unpinned);
            this.elem.classList.remove(this.classes.pinned);
        },

        /**
         * Pins the header if it's currently unpinned
         */
        pin: function () {
            this.elem.classList.remove(this.classes.unpinned);
            this.elem.classList.add(this.classes.pinned);
        },

        /**
         * Gets the Y scroll position
         * @see https://developer.mozilla.org/en-US/docs/Web/API/Window.scrollY
         * @return {Number} pixels the page has scrolled along the Y-axis
         */
        getScrollY: function () {
            return (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
        },

        /**
         * Handles updating the state of the widget
         */
        update: function () {
            var currentScrollY = this.getScrollY(),
              toleranceExceeded = Math.abs(currentScrollY - this.lastKnownScrollY) >= this.tolerance;

            if (currentScrollY < 0) { // Ignore bouncy scrolling in OSX
                return;
            }

            if (toleranceExceeded) {
                if (currentScrollY > this.lastKnownScrollY && currentScrollY >= this.offset) {
                    this.unpin();
                }
                else if (currentScrollY < this.lastKnownScrollY) {
                    this.pin();
                }
            }

            this.lastKnownScrollY = currentScrollY;
        }
    };
    /**
     * Default options
     * @type {Object}
     */
    Headroom.options = {
        tolerance: 0,
        offset: 0,
        classes: {
            pinned: 'headroom--pinned',
            unpinned: 'headroom--unpinned',
            initial: 'headroom'
        }
    };

    global.Headroom = Headroom;

}(this));
; (function () {

    function CodeGenerator(widgetCode, pluginCode, dataApiCode) {
        this.pluginCode = pluginCode;
        this.widgetCode = widgetCode;
        this.dataApiCode = dataApiCode;
    }
    CodeGenerator.prototype = {
        constructor: CodeGenerator,

        widget: function (options) {
            return 'var headroom = new Headroom(elem, ' + JSON.stringify(options, null, '  ') + ');\nheadroom.init();\n\n'
            + '// to destroy\n'
            + 'headroom.destroy();';
        },

        plugin: function (options) {
            return '$("header").headroom(' + JSON.stringify(options, null, '  ') + ');\n\n'
            + '// to destroy\n'
            + '$("#header").headroom("destroy");';
        },

        dataApi: function (options) {
            return '&lt;header data-headroom '
              + 'data-tolerance="' + options.tolerance + '" '
              + 'data-offset="' + options.offset + '" '
              + 'data-classes=\'' + JSON.stringify(options.classes) + '\'&gt;&lt;/header&gt;\n\n'
              + '// to destroy, in your JS:\n'
              + '$("header").data("headroom").destroy();';
        },

        generate: function (options) {
            this.pluginCode.innerHTML = this.plugin(options);
            Prism.highlightElement(this.pluginCode, false);

            this.widgetCode.innerHTML = this.widget(options);
            Prism.highlightElement(this.widgetCode, false);

            this.dataApiCode.innerHTML = this.dataApi(options);
            Prism.highlightElement(this.dataApiCode, false);
        }
    };


    window.CodeGenerator = CodeGenerator;

}());
/*!
 * headroom.js v0.3.11 - Give your page some headroom. Hide your header until you need it
 * Copyright (c) 2013 Nick Williams - http://wicky.nillia.ms/headroom.js
 * License: MIT
 */

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//js��ʼ��ҳ��
$(function () {
    var header = new Headroom(document.querySelector(".page-title"), {
        tolerance: 5,
        offset: 205,
        classes: {
            initial: "animated",
            pinned: "slideInDown",
            unpinned: "slideOutUp"
        }
    });
    header.init();  //page-title�������� ��ʾ
    // alert(0);
    $(".dropdown  a").each(function () { //��˵�������ǰ
        var uid = getQueryString("columnid");
        if (uid == "0") { uid = "index"; };
        $this = $(this);
        // alert($this[0].href);
        if ($this[0].href == String(window.location)) {

            $this.addClass("active");

        }

        if ("@url" == "/v/p") {
            if (($this[0].href.indexOf(String("@url")) > 1)) {
                $this.addClass("active");
                return false;
            }
        }
        if ("@url" == "/v/p/0") {
            if (($this[0].href.indexOf(String("@url")) > 1)) {
                $this.addClass("active");
            }
        }
    });

}());