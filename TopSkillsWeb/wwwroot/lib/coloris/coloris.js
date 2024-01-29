!function (e, t) {
    "function" == typeof define && define.amd ? define([], t) : "object" == typeof module && module.exports ? module.exports = t() : (e.Coloris = t(),
        "object" == typeof window && e.Coloris.init())
}("undefined" != typeof self ? self : void 0, function () {
    /*!
  * Copyright (c) 2021-2023 Momo Bassit.
  * Licensed under the MIT License (MIT)
  * https://github.com/mdbassit/Coloris
  * Version: 0.19.0
  * NPM: https://github.com/melloware/coloris-npm
  */
    return h = window,
        b = document,
        v = Math,
        i = b.createElement("canvas").getContext("2d"),
        O = {
            el: "[data-coloris]",
            parent: "body",
            theme: "default",
            themeMode: "light",
            rtl: !(H = {
                r: 0,
                g: 0,
                b: 0,
                h: 0,
                s: 0,
                v: 0,
                a: 1
            }),
            wrap: !0,
            margin: 2,
            format: "hex",
            formatToggle: !1,
            swatches: [],
            swatchesOnly: !1,
            alpha: !0,
            forceAlpha: !1,
            focusInput: !0,
            selectInput: !1,
            inline: !1,
            defaultColor: "#000000",
            clearButton: !1,
            clearLabel: "Clear",
            closeButton: !1,
            closeLabel: "Close",
            onChange: function () { },
            a11y: {
                open: "Open color picker",
                close: "Close color picker",
                clear: "Clear the selected color",
                marker: "Saturation: {s}. Brightness: {v}.",
                hueSlider: "Hue slider",
                alphaSlider: "Opacity slider",
                input: "Color value field",
                format: "Color format",
                swatch: "Color swatch",
                instruction: "Saturation and brightness selector. Use up, down, left and right arrow keys to select."
            }
        },
        D = {},
        I = "",
        j = !(N = {}),
        void 0 !== NodeList && NodeList.prototype && !NodeList.prototype.forEach && (NodeList.prototype.forEach = Array.prototype.forEach),
        (Z = function () {
            var r = {
                init: G,
                set: n,
                wrap: s,
                close: o,
                setInstance: W,
                setColor: J,
                removeInstance: q,
                updatePosition: c
            };
            function t(e) {
                $(function () {
                    e && ("string" == typeof e ? P : n)(e)
                })
            }
            for (var e in r)
                !function (a) {
                    t[a] = function () {
                        for (var e = arguments.length, t = new Array(e), l = 0; l < e; l++)
                            t[l] = arguments[l];
                        $(r[a], t)
                    }
                }(e);
            return $(function () {
                h.addEventListener("resize", function (e) {
                    t.updatePosition()
                }),
                    h.addEventListener("scroll", function (e) {
                        t.updatePosition()
                    })
            }),
                t
        }()).coloris = Z;
    function n(o) {
        if ("object" == typeof o)
            for (var i in o)
                !function () {
                    switch (i) {
                        case "el":
                            P(o.el),
                                !1 !== o.wrap && s(o.el);
                            break;
                        case "parent":
                            (y = b.querySelector(o.parent)) && (y.appendChild(g),
                                O.parent = o.parent,
                                y === b.body) && (y = null);
                            break;
                        case "themeMode":
                            O.themeMode = o.themeMode,
                                "auto" === o.themeMode && h.matchMedia && h.matchMedia("(prefers-color-scheme: dark)").matches && (O.themeMode = "dark");
                        case "theme":
                            o.theme && (O.theme = o.theme),
                                g.className = "clr-picker clr-" + O.theme + " clr-" + O.themeMode,
                                O.inline && c();
                            break;
                        case "rtl":
                            O.rtl = !!o.rtl,
                                b.querySelectorAll(".clr-field").forEach(function (e) {
                                    return e.classList.toggle("clr-rtl", O.rtl)
                                });
                            break;
                        case "margin":
                            o.margin *= 1,
                                O.margin = (isNaN(o.margin) ? O : o).margin;
                            break;
                        case "wrap":
                            o.el && o.wrap && s(o.el);
                            break;
                        case "formatToggle":
                            O.formatToggle = !!o.formatToggle,
                                f("clr-format").style.display = O.formatToggle ? "block" : "none",
                                O.formatToggle && (O.format = "auto");
                            break;
                        case "swatches":
                            var l;
                            Array.isArray(o.swatches) && (l = [],
                                o.swatches.forEach(function (e, t) {
                                    l.push('<button type="button" id="clr-swatch-' + t + '" aria-labelledby="clr-swatch-label clr-swatch-' + t + '" style="color: ' + e + ';">' + e + "</button>")
                                }),
                                f("clr-swatches").innerHTML = l.length ? "<div>" + l.join("") + "</div>" : "",
                                O.swatches = o.swatches.slice());
                            break;
                        case "swatchesOnly":
                            O.swatchesOnly = !!o.swatchesOnly,
                                g.setAttribute("data-minimal", O.swatchesOnly);
                            break;
                        case "alpha":
                            O.alpha = !!o.alpha,
                                g.setAttribute("data-alpha", O.alpha);
                            break;
                        case "inline":
                            O.inline = !!o.inline,
                                g.setAttribute("data-inline", O.inline),
                                O.inline && (e = o.defaultColor || O.defaultColor,
                                    M = R(e),
                                    c(),
                                    u(e));
                            break;
                        case "clearButton":
                            "object" == typeof o.clearButton && (o.clearButton.label && (O.clearLabel = o.clearButton.label,
                                x.innerHTML = O.clearLabel),
                                o.clearButton = o.clearButton.show),
                                O.clearButton = !!o.clearButton,
                                x.style.display = O.clearButton ? "block" : "none";
                            break;
                        case "clearLabel":
                            O.clearLabel = o.clearLabel,
                                x.innerHTML = O.clearLabel;
                            break;
                        case "closeButton":
                            O.closeButton = !!o.closeButton,
                                O.closeButton ? g.insertBefore(A, L) : L.appendChild(A);
                            break;
                        case "closeLabel":
                            O.closeLabel = o.closeLabel,
                                A.innerHTML = O.closeLabel;
                            break;
                        case "a11y":
                            var e, t, a = o.a11y, r = !1;
                            if ("object" == typeof a)
                                for (var n in a)
                                    a[n] && O.a11y[n] && (O.a11y[n] = a[n],
                                        r = !0);
                            r && (e = f("clr-open-label"),
                                t = f("clr-swatch-label"),
                                e.innerHTML = O.a11y.open,
                                t.innerHTML = O.a11y.swatch,
                                A.setAttribute("aria-label", O.a11y.close),
                                x.setAttribute("aria-label", O.a11y.clear),
                                S.setAttribute("aria-label", O.a11y.hueSlider),
                                C.setAttribute("aria-label", O.a11y.alphaSlider),
                                E.setAttribute("aria-label", O.a11y.input),
                                m.setAttribute("aria-label", O.a11y.instruction));
                            break;
                        default:
                            O[i] = o[i]
                    }
                }()
    }
    function W(e, t) {
        "string" == typeof e && "object" == typeof t && (D[e] = t,
            j = !0)
    }
    function q(e) {
        delete D[e],
            0 === Object.keys(D).length && (j = !1,
                e === I) && l()
    }
    function F(l) {
        if (j) {
            var a, r = ["el", "wrap", "rtl", "inline", "defaultColor", "a11y"];
            for (a in D)
                if ("break" === function () {
                    var t = D[a];
                    if (l.matches(a)) {
                        for (var e in I = a,
                            N = {},
                            r.forEach(function (e) {
                                return delete t[e]
                            }),
                            t)
                            N[e] = Array.isArray(O[e]) ? O[e].slice() : O[e];
                        return n(t),
                            "break"
                    }
                }())
                    break
        }
    }
    function l() {
        0 < Object.keys(N).length && (n(N),
            I = "",
            N = {})
    }
    function P(e) {
        a(b, "click", e, function (e) {
            O.inline || (F(e.target),
                B = e.target,
                r = B.value,
                M = R(r),
                g.classList.add("clr-open"),
                c(),
                u(r),
                (O.focusInput || O.selectInput) && E.focus({
                    preventScroll: !0
                }),
                O.selectInput && E.select(),
                (V || O.swatchesOnly) && K().shift().focus(),
                B.dispatchEvent(new Event("open", {
                    bubbles: !0
                })))
        }),
            a(b, "input", e, function (e) {
                var t = e.target.parentNode;
                t.classList.contains("clr-field") && (t.style.color = e.target.value)
            })
    }
    function c() {
        var e, t, l, a, r, n, o, i, c, s;
        g && (B || O.inline) && (e = y,
            t = h.scrollY,
            l = g.offsetWidth,
            a = g.offsetHeight,
            r = {
                left: !1,
                top: !1
            },
            o = {
                x: 0,
                y: 0
            },
            e && (i = h.getComputedStyle(e),
                n = parseFloat(i.marginTop),
                i = parseFloat(i.borderTopWidth),
                (o = e.getBoundingClientRect()).y += i + t),
            O.inline || (c = (i = B.getBoundingClientRect()).x,
                s = t + i.y + i.height + O.margin,
                e ? (c -= o.x,
                    s -= o.y,
                    c + l > e.clientWidth && (c += i.width - l,
                        r.left = !0),
                    s + a > e.clientHeight - n && a + O.margin <= i.top - (o.y - t) && (s -= i.height + a + 2 * O.margin,
                        r.top = !0),
                    s += e.scrollTop) : (c + l > b.documentElement.clientWidth && (c += i.width - l,
                        r.left = !0),
                        s + a - t > b.documentElement.clientHeight && a + O.margin <= i.top && (s = t + i.y - a - O.margin,
                            r.top = !0)),
                g.classList.toggle("clr-left", r.left),
                g.classList.toggle("clr-top", r.top),
                g.style.left = c + "px",
                g.style.top = s + "px",
                o.x += g.offsetLeft,
                o.y += g.offsetTop),
            w = {
                width: m.offsetWidth,
                height: m.offsetHeight,
                x: m.offsetLeft + o.x,
                y: m.offsetTop + o.y
            })
    }
    function s(e) {
        b.querySelectorAll(e).forEach(function (e) {
            var t, l, a = e.parentNode;
            a.classList.contains("clr-field") || (t = b.createElement("div"),
                l = "clr-field",
                (O.rtl || e.classList.contains("clr-rtl")) && (l += " clr-rtl"),
                t.innerHTML = '<button type="button" aria-labelledby="clr-open-label"></button>',
                a.insertBefore(t, e),
                t.setAttribute("class", l),
                t.style.color = e.value,
                t.appendChild(e))
        })
    }
    function o(e) {
        var t;
        B && !O.inline && (t = B,
            e && (B = null,
                r !== t.value) && (t.value = r,
                    t.dispatchEvent(new Event("input", {
                        bubbles: !0
                    }))),
            setTimeout(function () {
                r !== t.value && t.dispatchEvent(new Event("change", {
                    bubbles: !0
                }))
            }),
            g.classList.remove("clr-open"),
            j && l(),
            t.dispatchEvent(new Event("close", {
                bubbles: !0
            })),
            O.focusInput && t.focus({
                preventScroll: !0
            }),
            B = null)
    }
    function u(e) {
        var e = function (e) {
            var t;
            i.fillStyle = "#000",
                i.fillStyle = e,
                (e = /^((rgba)|rgb)[\D]+([\d.]+)[\D]+([\d.]+)[\D]+([\d.]+)[\D]*?([\d.]+|$)/i.exec(i.fillStyle)) ? (t = {
                    r: +e[3],
                    g: +e[4],
                    b: +e[5],
                    a: +e[6]
                }).a = +t.a.toFixed(2) : (e = i.fillStyle.replace("#", "").match(/.{2}/g).map(function (e) {
                    return parseInt(e, 16)
                }),
                    t = {
                        r: e[0],
                        g: e[1],
                        b: e[2],
                        a: 1
                    });
            return t
        }(e)
            , t = function (e) {
                var t = e.r / 255
                    , l = e.g / 255
                    , a = e.b / 255
                    , r = v.max(t, l, a)
                    , n = v.min(t, l, a)
                    , n = r - n
                    , o = r
                    , i = 0
                    , c = 0;
                n && (r === t && (i = (l - a) / n),
                    r === l && (i = 2 + (a - t) / n),
                    r === a && (i = 4 + (t - l) / n),
                    r) && (c = n / r);
                return {
                    h: (i = v.floor(60 * i)) < 0 ? i + 360 : i,
                    s: v.round(100 * c),
                    v: v.round(100 * o),
                    a: e.a
                }
            }(e);
        U(t.s, t.v),
            p(e, t),
            S.value = t.h,
            g.style.color = "hsl(" + t.h + ", 100%, 50%)",
            Q.style.left = t.h / 360 * 100 + "%",
            k.style.left = w.width * t.s / 100 + "px",
            k.style.top = w.height - w.height * t.v / 100 + "px",
            C.value = 100 * t.a,
            T.style.left = 100 * t.a + "%"
    }
    function R(e) {
        e = e.substring(0, 3).toLowerCase();
        return "rgb" === e || "hsl" === e ? e : "hex"
    }
    function d(e) {
        e = void 0 !== e ? e : E.value,
            B && (B.value = e,
                B.dispatchEvent(new Event("input", {
                    bubbles: !0
                }))),
            O.onChange && O.onChange.call(h, e),
            b.dispatchEvent(new CustomEvent("coloris:pick", {
                detail: {
                    color: e
                }
            }))
    }
    function Y(e, t) {
        var l, a, r, n, o, e = {
            h: +S.value,
            s: e / w.width * 100,
            v: 100 - t / w.height * 100,
            a: C.value / 100
        }, i = (i = (t = e).s / 100,
            l = t.v / 100,
            i *= l,
            a = t.h / 60,
            r = i * (1 - v.abs(a % 2 - 1)),
            i += l -= i,
            r += l,
            a = v.floor(a) % 6,
            n = [i, r, l, l, r, i][a],
            o = [r, i, i, r, l, l][a],
            l = [l, l, r, i, i, r][a],
        {
            r: v.round(255 * n),
            g: v.round(255 * o),
            b: v.round(255 * l),
            a: t.a
        });
        U(e.s, e.v),
            p(i, e),
            d()
    }
    function U(e, t) {
        var l = O.a11y.marker;
        e = +e.toFixed(1),
            t = +t.toFixed(1),
            l = (l = l.replace("{s}", e)).replace("{v}", t),
            k.setAttribute("aria-label", l)
    }
    function t(e) {
        var t = {
            pageX: ((t = e).changedTouches ? t.changedTouches[0] : t).pageX,
            pageY: (t.changedTouches ? t.changedTouches[0] : t).pageY
        }
            , l = t.pageX - w.x
            , t = t.pageY - w.y;
        y && (t += y.scrollTop),
            X(l, t),
            e.preventDefault(),
            e.stopPropagation()
    }
    function X(e, t) {
        e = e < 0 ? 0 : e > w.width ? w.width : e,
            t = t < 0 ? 0 : t > w.height ? w.height : t,
            k.style.left = e + "px",
            k.style.top = t + "px",
            Y(e, t),
            k.focus()
    }
    function p(e, t) {
        void 0 === t && (t = {});
        var l, a, r = O.format;
        for (l in e = void 0 === e ? {} : e)
            H[l] = e[l];
        for (a in t)
            H[a] = t[a];
        var n, o = function (e) {
            var t = e.r.toString(16)
                , l = e.g.toString(16)
                , a = e.b.toString(16)
                , r = "";
            e.r < 16 && (t = "0" + t);
            e.g < 16 && (l = "0" + l);
            e.b < 16 && (a = "0" + a);
            O.alpha && (e.a < 1 || O.forceAlpha) && (e = 255 * e.a | 0,
                r = e.toString(16),
                e < 16) && (r = "0" + r);
            return "#" + t + l + a + r
        }(H), i = o.substring(0, 7);
        switch (k.style.color = i,
        T.parentNode.style.color = i,
        T.style.color = o,
        L.style.color = o,
        m.style.display = "none",
        m.offsetHeight,
        m.style.display = "",
        T.nextElementSibling.style.display = "none",
        T.nextElementSibling.offsetHeight,
        T.nextElementSibling.style.display = "",
        "mixed" === r ? r = 1 === H.a ? "hex" : "rgb" : "auto" === r && (r = M),
        r) {
            case "hex":
                E.value = o;
                break;
            case "rgb":
                E.value = (n = H,
                    !O.alpha || 1 === n.a && !O.forceAlpha ? "rgb(" + n.r + ", " + n.g + ", " + n.b + ")" : "rgba(" + n.r + ", " + n.g + ", " + n.b + ", " + n.a + ")");
                break;
            case "hsl":
                E.value = (n = function (e) {
                    var t, l = e.v / 100, a = l * (1 - e.s / 100 / 2);
                    0 < a && a < 1 && (t = v.round((l - a) / v.min(a, 1 - a) * 100));
                    return {
                        h: e.h,
                        s: t || 0,
                        l: v.round(100 * a),
                        a: e.a
                    }
                }(H),
                    !O.alpha || 1 === n.a && !O.forceAlpha ? "hsl(" + n.h + ", " + n.s + "%, " + n.l + "%)" : "hsla(" + n.h + ", " + n.s + "%, " + n.l + "%, " + n.a + ")")
        }
        b.querySelector('.clr-format [value="' + r + '"]').checked = !0
    }
    function e() {
        var e = +S.value
            , t = +k.style.left.replace("px", "")
            , l = +k.style.top.replace("px", "");
        g.style.color = "hsl(" + e + ", 100%, 50%)",
            Q.style.left = e / 360 * 100 + "%",
            Y(t, l)
    }
    function z() {
        var e = C.value / 100;
        T.style.left = 100 * e + "%",
            p({
                a: e
            }),
            d()
    }
    function G() {
        b.getElementById("clr-picker") || (y = null,
            (g = b.createElement("div")).setAttribute("id", "clr-picker"),
            g.className = "clr-picker",
            g.innerHTML = '<input id="clr-color-value" name="clr-color-value" class="clr-color" type="text" value="" spellcheck="false" aria-label="' + O.a11y.input + '"><div id="clr-color-area" class="clr-gradient" role="application" aria-label="' + O.a11y.instruction + '"><div id="clr-color-marker" class="clr-marker" tabindex="0"></div></div><div class="clr-hue"><input id="clr-hue-slider" name="clr-hue-slider" type="range" min="0" max="360" step="1" aria-label="' + O.a11y.hueSlider + '"><div id="clr-hue-marker"></div></div><div class="clr-alpha"><input id="clr-alpha-slider" name="clr-alpha-slider" type="range" min="0" max="100" step="1" aria-label="' + O.a11y.alphaSlider + '"><div id="clr-alpha-marker"></div><span></span></div><div id="clr-format" class="clr-format"><fieldset class="clr-segmented"><legend>' + O.a11y.format + '</legend><input id="clr-f1" type="radio" name="clr-format" value="hex"><label for="clr-f1">Hex</label><input id="clr-f2" type="radio" name="clr-format" value="rgb"><label for="clr-f2">RGB</label><input id="clr-f3" type="radio" name="clr-format" value="hsl"><label for="clr-f3">HSL</label><span></span></fieldset></div><div id="clr-swatches" class="clr-swatches"></div><button type="button" id="clr-clear" class="clr-clear" aria-label="' + O.a11y.clear + '">' + O.clearLabel + '</button><div id="clr-color-preview" class="clr-preview"><button type="button" id="clr-close" class="clr-close" aria-label="' + O.a11y.close + '">' + O.closeLabel + '</button></div><span id="clr-open-label" hidden>' + O.a11y.open + '</span><span id="clr-swatch-label" hidden>' + O.a11y.swatch + "</span>",
            b.body.appendChild(g),
            m = f("clr-color-area"),
            k = f("clr-color-marker"),
            x = f("clr-clear"),
            A = f("clr-close"),
            L = f("clr-color-preview"),
            E = f("clr-color-value"),
            S = f("clr-hue-slider"),
            Q = f("clr-hue-marker"),
            C = f("clr-alpha-slider"),
            T = f("clr-alpha-marker"),
            P(O.el),
            s(O.el),
            a(g, "mousedown", function (e) {
                g.classList.remove("clr-keyboard-nav"),
                    e.stopPropagation()
            }),
            a(m, "mousedown", function (e) {
                a(b, "mousemove", t)
            }),
            a(m, "touchstart", function (e) {
                b.addEventListener("touchmove", t, {
                    passive: !1
                })
            }),
            a(k, "mousedown", function (e) {
                a(b, "mousemove", t)
            }),
            a(k, "touchstart", function (e) {
                b.addEventListener("touchmove", t, {
                    passive: !1
                })
            }),
            a(E, "change", function (e) {
                (B || O.inline) && (u(E.value),
                    d())
            }),
            a(x, "click", function (e) {
                d(""),
                    o()
            }),
            a(A, "click", function (e) {
                d(),
                    o()
            }),
            a(b, "click", ".clr-format input", function (e) {
                M = e.target.value,
                    p(),
                    d()
            }),
            a(g, "click", ".clr-swatches button", function (e) {
                u(e.target.textContent),
                    d(),
                    O.swatchesOnly && o()
            }),
            a(b, "mouseup", function (e) {
                b.removeEventListener("mousemove", t)
            }),
            a(b, "touchend", function (e) {
                b.removeEventListener("touchmove", t)
            }),
            a(b, "mousedown", function (e) {
                V = !1,
                    g.classList.remove("clr-keyboard-nav"),
                    o()
            }),
            a(b, "keydown", function (e) {
                var t, l = e.key, a = e.target, r = e.shiftKey;
                "Escape" === l ? o(!0) : ["Tab", "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].includes(l) && (V = !0,
                    g.classList.add("clr-keyboard-nav")),
                    "Tab" === l && a.matches(".clr-picker *") && (t = (l = K()).shift(),
                        l = l.pop(),
                        r && a === t ? (l.focus(),
                            e.preventDefault()) : r || a !== l || (t.focus(),
                                e.preventDefault()))
            }),
            a(b, "click", ".clr-field button", function (e) {
                j && l(),
                    e.target.nextElementSibling.dispatchEvent(new Event("click", {
                        bubbles: !0
                    }))
            }),
            a(k, "keydown", function (e) {
                var t = {
                    ArrowUp: [0, -1],
                    ArrowDown: [0, 1],
                    ArrowLeft: [-1, 0],
                    ArrowRight: [1, 0]
                };
                Object.keys(t).includes(e.key) && (!function (e, t) {
                    X(+k.style.left.replace("px", "") + e, +k.style.top.replace("px", "") + t)
                }
                    .apply(void 0, t[e.key]),
                    e.preventDefault())
            }),
            a(m, "click", t),
            a(S, "input", e),
            a(C, "input", z))
    }
    function K() {
        return Array.from(g.querySelectorAll("input, button")).filter(function (e) {
            return !!e.offsetWidth
        })
    }
    function f(e) {
        return b.getElementById(e)
    }
    function a(e, t, l, a) {
        var r = Element.prototype.matches || Element.prototype.msMatchesSelector;
        "string" == typeof l ? e.addEventListener(t, function (e) {
            r.call(e.target, l) && a.call(e.target, e)
        }) : (a = l,
            e.addEventListener(t, a))
    }
    function $(e, t) {
        t = void 0 !== t ? t : [],
            "loading" !== b.readyState ? e.apply(void 0, t) : b.addEventListener("DOMContentLoaded", function () {
                e.apply(void 0, t)
            })
    }
    function J(e, t) {
        r = (B = t).value,
            F(t),
            M = R(e),
            c(),
            u(e),
            d(),
            r !== e && B.dispatchEvent(new Event("change", {
                bubbles: !0
            }))
    }
    var h, b, v, y, g, m, w, k, L, E, x, A, S, Q, C, T, B, M, r, V, i, H, O, D, I, N, j, Z
});
