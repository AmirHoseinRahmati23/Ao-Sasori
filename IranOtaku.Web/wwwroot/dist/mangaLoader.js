/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./src/scss/mangaLoader.scss":
/*!***********************************!*\
  !*** ./src/scss/mangaLoader.scss ***!
  \***********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n// extracted by mini-css-extract-plugin\n\n\n//# sourceURL=webpack://manga-loader/./src/scss/mangaLoader.scss?");

/***/ }),

/***/ "./src/js/darkMode.js":
/*!****************************!*\
  !*** ./src/js/darkMode.js ***!
  \****************************/
/***/ (() => {

eval("const btnDarkMode = document.querySelector('#btn-dark-mode');\nfunction darkMode() {\n    const\n        body1 = document.querySelector('body'),\n        haeder1 = document.querySelector('.haeder'),\n        btnHome = document.querySelector('#btnHome'),\n        menu_con = document.querySelector('.menu-con'),\n        menu = document.querySelector('.menu'),\n        btnDarkMode = document.querySelector('#btn-dark-mode')\n    ;\n    body1.classList.toggle(\"bg-night\");\n    haeder1.classList.toggle(\"haeder-drck\");\n    btnHome.classList.toggle(\"svg-drck\");\n    btnHome.classList.toggle(\"btn-dark-settings\");\n    menu_con.classList.toggle(\"menu-con-dark\");\n    menu_con.classList.toggle(\"menu-con-bs-dark\");\n    menu.classList.toggle(\"menu-dark\");\n    btnDarkMode.classList.toggle(\"svg-drck\");\n}\nbtnDarkMode.addEventListener('click',darkMode);\n\n//# sourceURL=webpack://manga-loader/./src/js/darkMode.js?");

/***/ }),

/***/ "./src/js/index.js":
/*!*************************!*\
  !*** ./src/js/index.js ***!
  \*************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _scss_mangaLoader_scss__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../scss/mangaLoader.scss */ \"./src/scss/mangaLoader.scss\");\n/* harmony import */ var _sht_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./sht.js */ \"./src/js/sht.js\");\n/* harmony import */ var _sht_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_sht_js__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var _darkMode_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./darkMode.js */ \"./src/js/darkMode.js\");\n/* harmony import */ var _darkMode_js__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_darkMode_js__WEBPACK_IMPORTED_MODULE_2__);\n\n\n\n\n\n(function () {\n    const app = document.querySelector(\"#app\");\n    const chCantent = document.querySelector(\"#chCantent\");\n    let arrayIndex = 0;\n    fetch(url)\n        .then(res => {\n            if (res.ok) {\n                return res.json();\n            }else {\n                throw Error(res.status);\n            }\n        })\n        .then(deta => {\n                    deta.images.forEach(j => {\n                        let img = document.createElement('img');\n                        img.src = \"https://dl.aosasori.com/ChapterParts/\" + j;\n                        app.appendChild(img);\n                    });\n        })\n        .catch(err => {\n            console.log(err.message);\n        })\n})();\n\n\n//# sourceURL=webpack://manga-loader/./src/js/index.js?");

/***/ }),

/***/ "./src/js/sht.js":
/*!***********************!*\
  !*** ./src/js/sht.js ***!
  \***********************/
/***/ (() => {

eval("const \n    btn_search = document.querySelector('.btn-search'),\n    menu = document.querySelector('.menu'),\n    btn_close = document.querySelector('.btn-close'),\n    menu_con = document.querySelector('.menu-con')\n;\nfunction show(btn,con,close,s) {\n    btn.onclick = () => {\n        con.classList.toggle(\"d-block\");\n        s.classList.remove(\"menu-con-bs\");\n    }\n    close.onclick = () => {\n        con.classList.toggle(\"d-block\");\n        s.classList.add(\"menu-con-bs\");\n    }\n}\nshow(btn_search,menu,btn_close,menu_con);\n\n//# sourceURL=webpack://manga-loader/./src/js/sht.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = __webpack_require__("./src/js/index.js");
/******/ 	
/******/ })()
;