HaselApp.directive('pinToTop', ['$rootScope', function ($rootScope) {
    return {
        restrict: 'A',
        scope: {
            Options: '=pinToTop'
        },
        link: function (scope, element, attrs) {
            //console.log("pinToTop", scope.Options);

            var w = angular.element(window);
            w.bind("scroll", function () {
                var top = scope.Options.top;
                var left = scope.Options.left;
                var right = scope.Options.right;
                var defaultLeft = scope.Options.defaultLeft;
                var scroll = scope.Options.scroll;
                var color = scope.Options.color;
                if ($(window).scrollTop() > (scroll ? scroll : 60)) {
                    //console.log("pinToTop.scroll", $(window).scrollTop(), scope.Options);
                    element.css({
                        'position': 'fixed',
                        'top': (top ? top : '13') + 'px',
                        'margin-left': (left >= 0 ? left : '30') + 'px',
                        'z-index': '1030',
                    });
                    if (right >= 0)
                        element.css({ 'right': right + 'px' });
                    if (color !== undefined)
                        element.css({ 'color': color });

                    if (!$rootScope.Viewport.is("lg")) {
                        element.children().find("[hiddenTop]").removeClass("hidden");
                        element.children().find("[hiddenTop]").addClass("hidden");
                    }
                } else {
                    element.css({
                        'position': 'relative',
                        'top': 'auto',
                        'margin-left': defaultLeft + 'px',
                        'z-index': '501',
                    });
                    if (right >= 0)
                        element.css({ 'right': 'auto' });
                    if (color !== undefined)
                        element.css({ 'color': '' });

                    if (!$rootScope.Viewport.is("lg"))
                        element.children().find("[hiddenTop]").removeClass("hidden");
                }
            });
        }
    };
}]);

HaselApp.directive('resize', ['$window', function ($window) {
    return {
        link: function (scope) {
            function onResize(e) {
                // Namespacing events with name of directive + event to avoid collisions
                scope.$broadcast('resize::resize');
            }

            function cleanUp() {
                angular.element($window).off('resize', onResize);
            }

            angular.element($window).on('resize', onResize);
            scope.$on('$destroy', cleanUp);
        }
    }
}]);

HaselApp.directive('validationMessage', function () {
    return {
        transclude: true,
        scope: { ctrl: '=validationMessage' },
        controller: ["$scope", "$element", "$attrs", "$timeout", function ($scope, $element, $attrs, $timeout) {
            $scope.touched = false;
            $timeout(function () {
                if (typeof $scope.ctrl != 'undefined')
                    $scope.$watchCollection("ctrl", function (e) {
                        var parentElement = $element.parent("div");
                        if (parentElement !== null || parentElement !== undefined) {
                            //****************basicSelect, searchSelect adaptation**************\\
                            parentElement.find("input[class*='ui-select-search']").bind("change", function () {
                                e.$touched = true;
                                e.$untouched = false;
                            });

                            parentElement.find("div[class*='ui-select-match'],div[class*='ui-select-button'],input[class*='ui-select-search']").bind("click", function () {
                                e.$touched = true;
                                e.$untouched = false;
                            });

                            //****************basicSelect, searchSelect adaptation**************\\

                            if (!e.$valid && e.$touched)
                                parentElement.addClass('has-error');
                            else
                                parentElement.removeClass('has-error');

                            function AddErrorClass(e) {
                                if ((!e.start.$valid && e.start.$touched) || (!e.end.$valid && e.end.$touched))
                                    parentElement.addClass('has-error');
                                else
                                    parentElement.removeClass('has-error');
                            }
                            if (typeof $scope.ctrl.start != 'undefined' && typeof $scope.ctrl.end != 'undefined') {
                                var inputGroup = $element.parents("[class*='input-daterange']");
                                if (typeof inputGroup !== 'undefined') {
                                    $scope.$watchCollection("ctrl.start", function (e) {
                                        $scope.touched = e.$touched;
                                        AddErrorClass($scope.ctrl);
                                    });

                                    $scope.$watchCollection("ctrl.end", function (e) {
                                        $scope.touched = e.$touched;
                                        AddErrorClass($scope.ctrl);
                                    });
                                }
                            }
                        }
                        if (typeof e.$touched != 'undefined')
                            $scope.touched = e.$touched;
                    });
            });
        }],
        template: '<div class="text-danger help-block" ng-messages="(ctrl.$error || ctrl.start.$error || ctrl.end.$error)" ng-transclude></div>'
    };
})
    .directive('required', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            template: '<p class="field-validation-valid text-danger" ng-message="required" ng-show="$parent.$parent.touched">Bu alan gereklidir.</p>'
        };
    })
    .directive('email', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            template: '<p class="field-validation-valid text-danger" ng-message="email" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.EnterValidEmail </p>'
        };
    })
    .directive('minlength', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            template: '<p class="field-validation-valid text-danger" ng-message="minlength" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.ThisFieldIsToShort</p>'
        };
    })
    .directive('maxlength', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            template: '<p class="field-validation-valid text-danger " ng-message="maxlength" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.ThisFieldIsToLong</p>'
        };
    })
    .directive('url', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            template: '<p class="field-validation-valid text-danger" ng-message="url" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.EnterValidUrl</p>'
        };
    })
    .directive('date', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.date = function (mValue, vValue) {
                    var value = vValue;
                    return (Object.prototype.toString.call(mValue) === "[object Date]" || value == undefined || value == "") ? true : /^(0?[1-9]|[12][0-9]|3[01])[\/\-\.](0?[1-9]|1[012])[\/\-\.]\d{2}$/.test(value) || /^(0?[1-9]|[12][0-9]|3[01])[\/\-\.](0?[1-9]|1[012])[\/\-\.]\d{4}$/.test(value);
                }
            },
            template: '<p class="field-validation-valid text-danger" ng-message="date" ng-if="$parent.$parent.touched">Geçerli bir tarih giriniz</p>'
        }
    })
    .directive('number', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.number = function (mValue, vValue) {
                    var reg = /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/;
                    var value = mValue || vValue;
                    return (value == undefined || value == "") ? true : reg.test(value);
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="number" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.EnterValidNumberFormat</p>'
        }
    })
    .directive('rangeLength', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.rangelength = function (mValue, vValue) {
                    var value = mValue || vValue;
                    return (value == undefined || value == "") ? true : (value.length >= scope.params.p0 && value.length <= scope.params.p1);
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="rangelength" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.ValidRangeLength</p>'
        }
    })
    .directive('equalTo', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { object: '=', params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.equalto = function (mValue, vValue) {
                    var value = mValue || vValue;
                    return (value == undefined || value == "") ? true : (value == scope.object);
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="equalto" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.ValidEqualTo</p>'
        }
    })
    .directive('letterRequired', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.letterRequired = function (mValue, vValue) {
                    var regex = /^\d*[a-zA-Z][a-zA-Z0-9_=[\]\\\W]*$/;
                    var value = mValue || vValue;
                    return (value == undefined || value == "") ? true : (value.search(regex) > -1);
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="letterRequired" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.AtLeastOneLetter</p>'
        }
    })
    .directive('datePickerValidation', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                function GetDate(value) {
                    var char = (typeof scope.params == 'undefined' || typeof scope.params.char == 'undefined') ? '.' : scope.param.char;
                    var date = value.split(char);
                    var d = parseInt(date[0], 10),
                        m = parseInt(date[1], 10),
                        y = parseInt(date[2], 10);
                    var ret = {
                        date: new Date(y, m - 1, d),
                        day: d,
                        month: m,
                        year: y
                    }
                    return ret;
                }

                function IsValidDate(value) {
                    var dateInfo = GetDate(value);
                    if (dateInfo.year < 1970 || dateInfo.month < 1 || dateInfo.month > 12 || dateInfo.day < 1 || dateInfo.day > 31)
                        return false;
                    return !/Invalid|NaN/.test(dateInfo.date);
                }

                if (typeof scope.$parent.$parent.ctrl.start != 'undefined')
                    scope.$parent.$parent.ctrl.start.$validators.date = function (mValue, vValue) {
                        var value = vValue;
                        var compareDate = GetDate(element.parents("[class*='form-group']").find("[data-type='end']").val()).date;
                        var ret = (value == undefined || value == "") ? true : IsValidDate(value) && (GetDate(value).date <= compareDate);
                        return ret;
                    }
                if (typeof scope.$parent.$parent.ctrl.end != 'undefined')
                    scope.$parent.$parent.ctrl.end.$validators.date = function (mValue, vValue) {
                        var value = vValue;
                        var compareDate = GetDate(element.parents("[class*='form-group']").find("[data-type='begin']").val()).date;
                        var ret = (value == undefined || value == "") ? true : IsValidDate(value) && (GetDate(value).date >= compareDate);
                        return ret;
                    }
                if (typeof scope.$parent.$parent.ctrl.$validators != 'undefined')
                    scope.$parent.$parent.ctrl.$validators.date = function (mValue, vValue) {
                        var value = vValue;
                        var ret = (value == undefined || value == "") ? true : IsValidDate(value);
                        return ret;
                    }
            },
            template: '<p class="field-validation-valid text-danger date-validation" ng-message="date" ng-if="$parent.$parent.touched" translate translate-values=\'{{ params }}\'>.Validation.EnterValidDate</p>'
        }
    })
    .directive('requiredFiles', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                var list = scope.params.split(',');
                scope.$parent.$parent.ctrl.$validators.requiredFiles = function (mValue, vValue) {
                    var missingList = [];
                    for (var i = 0; i < list.length; i++) {
                        var found = false;
                        if (mValue != null)
                            for (var j = 0; j < mValue.length; j++) {
                                if (mValue[j].name.search(list[i]) > -1) {
                                    found = true;
                                    break;
                                }
                            }
                        if (found) continue;
                        missingList.push("." + list[i]);
                    }
                    scope.messageParam = { p0: "<code>" + missingList.join('</code>,<code>') + "</code>" };
                    return (mValue == undefined || mValue == []) ? true : missingList.length == 0;
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="requiredFiles" ng-if="$parent.$parent.touched" translate translate-values=\'{{ messageParam }}\'>.Validation.RequiredFiles</p>'
        }
    })
    .directive('minNumber', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.minNumber = function (mValue, vValue) {
                    var value = mValue || vValue;
                    return (value == undefined || value == "") ? true : value >= scope.params;
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="minNumber" ng-if="$parent.$parent.touched">Girilen değer {{params-1}} değerinden büyük olmalıdır.</p>'
        }
    })
    .directive('maxNumber', function () {
        return {
            restrict: 'E',
            require: '^^validationMessage',
            scope: { params: '=' },
            link: function (scope, element, attr, ctrls) {
                scope.$parent.$parent.ctrl.$validators.maxNumber = function (mValue, vValue) {
                    var value = mValue || vValue;
                    return (value == undefined || value == "") ? true : value <= scope.params;
                }
            },
            template: '<p class="field-validation-valid text-danger help-block" ng-message="maxNumber" ng-if="$parent.$parent.touched">Girilen değer {{params-1}} değerinden küçük olmalıdır.</p>'
        }
    });

HaselApp.directive('datePicker', ['$timeout', function ($timeout) {
    return {
        scope: {
            ngModel: '=',
            options: '=',
            begin: '=',
            end: '='
        },
        link: function (scope, element) {
            if (typeof scope.options === 'undefined')
                scope.options = {};

            var datePickerModelChangeTimer = null;
            scope.RunOnChange = function () {
                if (typeof scope.options.onChange !== 'undefined' && typeof scope.options.onChange == 'function') {
                    if (datePickerModelChangeTimer)
                        $timeout.cancel(datePickerModelChangeTimer);
                    if (typeof scope.options.onChangeFilter != 'undefined')
                        datePickerModelChangeTimer = $timeout(function () {
                            scope.options.onChange(scope.options.onChangeFilter);
                        }, 800);
                    else
                        datePickerModelChangeTimer = $timeout(function () {
                            scope.options.onChange();
                        }, 800);
                }
            }

            jQuery(element).datepicker({
                language: activeLanguageAbbr,
                weekStart: 1,
                autoclose: true,
                todayHighlight: true,
                clearBtn: true,
                todayBtn: true,
                format: 'dd.mm.yyyy'
            })
                .on("changeDate", function (e) {
                    var type = jQuery(e.target).data("type");

                    switch (type) {
                        case 'begin':
                            scope.begin = element.find("input[data-type='begin']").val();
                            break;
                        case 'end':
                            scope.end = element.find("input[data-type='end']").val();
                            break;
                        default:
                            if (element.is("input[type='text']"))
                                scope.ngModel = element.val();
                            break;
                    }
                    scope.$applyAsync();
                    scope.RunOnChange();
                }).on("clearDate", function (e) {
                    var type = jQuery(e.target).data("type");

                    switch (type) {
                        case 'begin':

                            scope.begin = element.find("input[data-type='begin']").val();
                            break;
                        case 'end':
                            scope.end = element.find("input[data-type='end']").val();
                            break;
                        default:
                            if (element.is("input[type='text']"))
                                scope.ngModel = element.val();
                            break;
                    }
                    scope.$applyAsync();
                    scope.RunOnChange();
                });
            $timeout(function () {
                var beginInput = element.find("input[data-type='begin']");
                if (typeof beginInput != 'undefined') {
                    jQuery(beginInput).datepicker('update', moment(scope.begin).format("DD.MM.YYYY"));
                    scope.$watch('begin', function () {
                        if (typeof scope.begin != 'undefined')
                            jQuery(beginInput).datepicker('update', scope.begin);
                    });
                }

                var endInput = element.find("input[data-type='end']");
                if (typeof endInput != 'undefined') {
                    jQuery(endInput).datepicker('update', moment(scope.end).format("DD.MM.YYYY"));
                    scope.$watch('end', function () {
                        if (typeof scope.end != 'undefined')
                            jQuery(endInput).datepicker('update', scope.end);
                    });
                }

                if (element.is("input[type='text']")) {
                    jQuery(element).datepicker('update', moment(scope.ngModel).format("DD.MM.YYYY"));
                    scope.$watch('ngModel', function () {
                        if (typeof scope.ngModel != 'undefined')
                            jQuery(element).datepicker('update', scope.ngModel);
                    });
                }
            });
        }
    };
}]);

HaselApp.directive('datetimePicker', ['$timeout', function ($timeout) {
    return {
        scope: {
            ngModel: '=',
            options: '=',
            begin: '=',
            end: '='
        },
        link: function (scope, element, attrs) {
            var changingDate;
            if (typeof !scope.options === 'undefined')
                scope.options = {};
            if (!scope.options.format)
                scope.options.format = 'DD.MM.YYYY HH:mm';
            var datePickerModelChangeTimer = null;
            scope.RunOnChange = function () {
                if (typeof scope.options.onChange == 'function') {
                    if (datePickerModelChangeTimer)
                        $timeout.cancel(datePickerModelChangeTimer);
                    if (typeof scope.options.onChangeFilter != 'undefined')
                        datePickerModelChangeTimer = $timeout(function () {
                            scope.options.onChange(scope.options.onChangeFilter);
                        }, 800);
                    else
                        datePickerModelChangeTimer = $timeout(function () {
                            scope.options.onChange(changingDate);
                        }, 800);
                }
            }
            jQuery(element).datetimepicker({
                format: scope.options.format,
                useCurrent: scope.options.useCurrent ? scope.options.useCurrent : false,
                locale: activeLanguageAbbr,
                showTodayButton: scope.options.showTodayButton ? scope.options.showTodayButton : false,
                showClear: scope.options.showClear ? scope.options.showClear : true,
                showClose: scope.options.showClose ? scope.options.showClose : false,
                sideBySide: scope.options.sideBySide ? scope.options.sideBySide : false,
                inline: scope.options.inline ? scope.options.inline : false,
                keepOpen: scope.options.keepOpen ? scope.options.keepOpen : false,
                icons: {
                    time: 'si si-clock',
                    date: 'si si-calendar',
                    up: 'si si-arrow-up',
                    down: 'si si-arrow-down',
                    previous: 'si si-arrow-left',
                    next: 'si si-arrow-right',
                    today: 'si si-size-actual',
                    clear: 'si si-trash',
                    close: 'si si-close'
                }
            }).on("dp.change", function (e) {
                var type = jQuery(e.target).data("type");

                switch (type) {
                    case 'begin':
                        scope.begin = element.find("input[data-type='begin']").val();

                        break;
                    case 'end':
                        changingDate = element.find("input[data-type='end']").val();

                        break;
                    default:
                        if (element.is("input[type='text']"))
                            if (scope.ngModel != element.val()) {
                                scope.ngModel = element.val();
                            }
                        break;
                }
                changingDate = e.date;
                scope.$applyAsync();
                scope.RunOnChange();
            });

            $timeout(function () {
                var beginInput = element.find("input[data-type='begin']");
                if (typeof beginInput != 'undefined' && beginInput.length > 0) {
                    jQuery(beginInput).data("DateTimePicker").date(moment(scope.begin).format(scope.options.format));
                    scope.$watch('begin', function () {
                        if (typeof scope.begin != 'undefined')
                            jQuery(beginInput).data("DateTimePicker").date(moment(scope.begin).format(scope.options.format));
                    });
                }
                var endInput = element.find("input[data-type='end']");
                if (typeof endInput != 'undefined' && endInput.length > 0) {
                    jQuery(endInput).data("DateTimePicker").date(moment(scope.end).format(scope.options.format));
                    scope.$watch('end', function () {
                        if (typeof scope.end != 'undefined')
                            jQuery(beginInput).data("DateTimePicker").date(scope.end);
                    });
                }
                if (element.is("input[type='text']")) {
                    jQuery(element).data("DateTimePicker").date(moment(scope.ngModel).format(scope.options.format));
                    scope.$watch('ngModel', function () {
                        if (typeof scope.ngModel != 'undefined')
                            jQuery(element).data("DateTimePicker").date(scope.ngModel);
                    });
                }
            });
        }
    };
}]);

HaselApp.directive('tooltip', function () {
    return {
        /*
        place="top"
        place="left"
        place="right"
        place="bottom"
        */
        link: function (scope, element, attr) {
            //console.log("tooltip");

            element.attr("data-toggle", "tooltip");

            if (typeof attr.container !== 'undefined')
                element.attr("data-container", attr.container);

            if (typeof attr.place !== 'undefined')
                element.attr("data-placement", attr.place);
            element.removeClass("tooltips");
            element.addClass("tooltips");
            element.attr("title", attr.tooltip);

            $(element).tooltip();
        }
    };
});

HaselApp.directive('drowpdownButton', function () {
    return {
        /*
        place="top"
        place="left"
        place="right"
        place="bottom"
        */
        link: function (scope, element, attr) {
            ////console.log("tooltip");

            //element.attr("data-toggle", "tooltip");

            //if (typeof attr.container !== 'undefined')
            //    element.attr("data-container", attr.container);

            //if (typeof attr.place !== 'undefined')
            //    element.attr("data-placement", attr.place);
            //element.removeClass("tooltips");
            //element.addClass("tooltips");
            //element.attr("title", attr.tooltip);

            //$(element).tooltip();
        }
    };
});
HaselApp.directive('placeholder', function () {
    return {
        link: function (scope, element, attr) {
            var placeHolder = element.attr("placeholder")

            element.attr("placeholder", placeHolder);
        }
    };
});

HaselApp.directive('splitCamel', ['$timeout', function ($timeout) {
    return {
        link: function (scope, element, attr) {
            $timeout(function () {
                console.log(element.text());
                var res = (element.text()).replace(/([A-Z]+|[0-9]+)/g, ' $1');
                element.text(res);
            });
        }
    };
}]);

HaselApp.directive('blockOption', function () {
    return {
        link: function (scope, element) {
            var el = jQuery(element);
            var block = null;

            // Init Icons
            if (typeof element.attr("init-icon") !== 'undefined')
                scope.Helpers.uiBlocks(false, 'init', el);

            // Call blocks API on click
            el.on('click', function () {
                var target = element.data("target");
                if (typeof target === 'undefined')
                    block = el.closest('.block');
                else
                    block = angular.element('#' + target);
                scope.Helpers.uiBlocks(block, el.data('action'));
                scope.$applyAsync();
            });
        }
    };
});

HaselApp.directive('showLoader', function () {
    return {
        scope: { showLoader: '=' },
        link: function (scope, element, attr) {
            scope.$watchCollection("showLoader", function (e) {
                if (e)
                    element.removeClass("block-opt-refresh").addClass("block-opt-refresh");
                else
                    element.removeClass("block-opt-refresh");
            });
        }
    };
});

HaselApp.directive('basicSelect', [
    function () {
        return {
            restrict: 'EA',
            scope: {
                ngModel: '=',
                ngEnabled: '=',
                options: '=',
                ngChange: '=',
                multiple: '=',
                modelValue: '=',
                reqired: '='
            },
            templateUrl: '/Content/HtmlTemplates/BasicSelectTemplate.html?v=1',

            controller: ["$scope", "$attrs", "$element", "$timeout", "selectHelper", function ($scope, $attrs, $element, $timeout, selectHelper) {
                if (typeof $scope.options === 'undefined')
                    $scope.options = {};
                selectHelper.InitDirectiveScope($scope, $attrs, $element);

                if (typeof $scope.options.data === 'undefined')
                    $scope.options.data = [];

                $scope.$watch("options.data", function (v) {
                    if (v && v.length == 0) {
                        $scope.params.Model = null;
                    }
                });

                $scope.FireDirections = function (direction) {
                    if (angular.isFunction($scope.options.FireDirections))
                        $scope.options.FireDirections(direction);
                };

                $scope.GroupBy = function (item) {
                    if (item.Group) {
                        if (item.Group == 0)
                            return '';
                        return 'Grup: ' + item.Group;
                    };
                };
                if (typeof $scope.options.onLoad !== 'undefined' && typeof $scope.options.onLoad === 'function')
                    $scope.options.onLoad();
            }]
        }
    }
]);

HaselApp.directive('searchSelect', [
    function () {
        return {
            restrict: 'EA',
            scope: {
                ngModel: '=',
                ngEnabled: '=',
                options: '=',
                ngChange: '=',
                multiple: '=',
                onChange: '=',
                onSearch: '=',
                onAdding: '&',
                reqired: '=',
                ngSyncValue: '='
            },
            templateUrl: '/Content/HtmlTemplates/SearchSelectTemplate.html?v=6',

            controller: ["$scope", "$attrs", "$element", "$timeout", "selectHelper", "$notification", function ($scope, $attrs, $element, $timeout, selectHelper, $notification) {
              
                if (typeof $scope.options === 'undefined')
                    $scope.options = {};
                if (typeof $scope.options.nested == 'undefined')
                    $scope.options.nested = false;

                if (typeof $scope.options.AllowEmptyModel == 'undefined')
                    $scope.options.AllowEmptyModel = true;

                selectHelper.InitDirectiveScope($scope, $attrs, $element);
                if (typeof $scope.options.data === 'undefined')
                    $scope.options.data = [];
                $scope.AddItem = function () {
                    $scope.onAdding();
                }

                var searchSelecModeltimer = null;
                $scope.$watch('ngModel', function () {
                    if (typeof $scope.options.onChange == 'function') {
                        if (searchSelecModeltimer)
                            $timeout.cancel(searchSelecModeltimer);
                        if (typeof $scope.options.onChangeFilter != 'undefined')
                            searchSelecModeltimer = $timeout(function () {
                                $scope.options.onChange($scope.options.onChangeFilter);
                            }, 10);
                        else
                            $timeout(function () {
                                $scope.options.onChange($scope.ngModel);
                            }, 10);
                    }
                });

                if (typeof $scope.options.DataSource !== 'undefined' && typeof $scope.options.DataSource !== 'function') {
                    $scope.options.data = $scope.options.DataSource;
                }

                if (typeof $scope.options.AllowSearch == 'undefined')
                    $scope.options.AllowSearch = true;

                $scope.triggerDataSourceBound = function () {
                    if (typeof $scope.options.onDataBound == 'function')
                        $scope.options.onDataBound();
                }

                $scope.SuccessFn = function (result) {
                    $scope.options.IsLoading = false;
                    if (result.IsSuccess) {
                        $scope.options.data = result.Data;
                    } else {
                        $scope.WarningMessage = result.Message == null ? "Tanımlanamayan bir hata oluştu" : result.Message;
                        $notification.Notify("Hata", $scope.WarningMessage, "d");
                    }
                    $scope.triggerDataSourceBound();
                }

                $scope.ErrorFn = function (result) {
                    $scope.options.IsLoading = false;
                    $notification.Notify("Hata", result.statusText, "d");
                    $scope.triggerDataSourceBound();
                }

                $scope.options.GetData = function () {
                    if (typeof $scope.options.DeferDataSource !== 'undefined')
                        $scope.options.DeferDataSource($scope.options.BindingParams).then($scope.SuccessFn);

                    if (typeof $scope.options.DataSource !== 'undefined' && typeof $scope.options.DataSource === 'function') {
                        $scope.options.IsLoading = true;
                        if (typeof $scope.options.SearchFilter != 'undefined')
                            $scope.options.DataSource($scope.options.BindingParams, angular.copy($scope.options.SearchFilter), $scope.SuccessFn, $scope.ErrorFn);
                        else
                            $scope.options.DataSource($scope.options.BindingParams, $scope.SuccessFn, $scope.ErrorFn);
                    }
                }

                $scope.Search = function (v) {
                    $scope.options.SearchText = v;
                    if (!v || !$scope.options.AllowSearch)
                        return;

                    if (typeof $scope.onSearch == 'function') {
                        $scope.options.IsLoading = true;

                        if (typeof $scope.options.SearchFilter != 'undefined')
                            $scope.onSearch(v, angular.copy($scope.options.SearchFilter), $scope.SuccessFn, $scope.ErrorFn);
                        else
                            $scope.onSearch(v, $scope.SuccessFn, $scope.ErrorFn);
                    }
                }
                if (typeof $scope.options.onLoad !== 'undefined' && typeof $scope.options.onLoad === 'function')
                    $scope.options.onLoad();

                if (typeof $scope.options.BindOnLoad === 'undefined' || $scope.options.BindOnLoad === true)
                    $scope.options.GetData();

                $scope.options.GetModel = function (value) {
                    for (var i = 0; i < $scope.options.data.length; i++) {
                        var model = $scope.options.data[i];
                        if (model[$scope.options.valueField] === value)
                            return model;
                    }
                    return null;
                }
            }]
        }
    }
]);

HaselApp.directive('fileUpload', ['$compile', '$notification', function ($compile, $notification) {
    return {
        scope: { ngModel: '=', options: '=', visibility: '=' },
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            if (typeof scope.options == 'undefined') {
                scope.options = {};
                scope.options.onlyacceptedfiles = false;
            }
            if (typeof scope.options.showButton == 'undefined') {
                scope.options.showButton = true;
            }

            if (typeof scope.options.icon == 'undefined') {
                scope.options.icon = "fa fa-upload";
            }
            var fileLabel = angular.element("<label for='" + element.attr("name") + "' class='btn btn-default' tooltip='Header.UploadFiles'><i class='" + scope.options.icon + "'></i></label>");
            if (!scope.options.showButton) {
                element.parent().append(fileLabel);
                element.css("display", "none");
                $compile(fileLabel)(scope);
            }

            ngModel.$render = function (files) {
                if (files != null && files.length == 0)
                    files = null;
                ngModel.$setViewValue(files);
            };

            element.bind('change', function (event) {
                var files = [];

                scope.options.ResetInput = function () {
                    element.val(null);
                }
                if (scope.options.onlyacceptedfiles) {
                    var types = element.attr("accept").split(",");
                    for (var i = 0; i < event.target.files.length; i++) {
                        var isAcceptedFile = false;
                        for (var j = 0; j < types.length; j++) {
                            if (event.target.files[i].name.toLowerCase().endsWith(types[j].trim().toLowerCase())) {
                                files.push(event.target.files[i]);
                                isAcceptedFile = true;
                                continue;
                            }
                        }
                        if (!isAcceptedFile) {
                            $notification.Notify("Hata", "Kısıtlanmış dosya tipi algılandı. Dosya: {0}".replace("{0}", event.target.files[i].name), "d");
                            files = [];
                            scope.options.ResetInput();
                            break;
                        }
                    }
                } else
                    files = event.target.files;
                if (typeof scope.options.onSelectionChanged != "undefined" && typeof scope.options.onSelectionChanged == "function")
                    scope.options.onSelectionChanged(files);

                scope.$apply(function () {
                    ngModel.$render(files);
                });
            });
        }
    };
}]);

HaselApp.directive('tableCheckable', function () {
    return {
        scope: { ngModel: '=', options: '=' },
        link: function (scope, element) {
            if (typeof scope.options == 'undefined')
                scope.options = {};

            if (typeof scope.options.selections == 'undefined')
                scope.$applyAsync(scope.option.selections = []);
            var table = jQuery(element);

            scope.options.reloadTable = function () {
                jQuery('tbody input:checkbox', table).each(function () {
                    var checkbox = jQuery(this);

                    checkbox.prop('checked', false);
                    uiCheckRow(checkbox, false);
                });
            }
            jQuery('thead input:checkbox', table).change(function () {
                var checkedStatus = jQuery(this).prop('checked');
                jQuery('tbody input:checkbox', table).each(function () {
                    var checkbox = jQuery(this);

                    checkbox.prop('checked', checkedStatus);
                    uiCheckRow(checkbox, checkedStatus);
                });
            });

            scope.$watchCollection("ngModel", function () {
                jQuery('tbody input:checkbox', table).change(function () {
                    var checkbox = jQuery(this);

                    uiCheckRow(checkbox, checkbox.prop('checked'));
                });

                jQuery('tbody > tr', table).click(function (e) {
                    if (e.target.type !== 'checkbox' &&
                        e.target.type !== 'button' &&
                        e.target.tagName.toLowerCase() !== 'a' &&
                        !jQuery(e.target).parent('label').length) {
                        var checkbox = jQuery('input:checkbox', this);
                        var checkedStatus = checkbox.prop('checked');

                        checkbox.prop('checked', !checkedStatus);
                        uiCheckRow(checkbox, !checkedStatus);
                    }
                });
            });

            var uiCheckRow = function (checkbox, checkedStatus) {
                if (checkedStatus) {
                    checkbox
                        .closest('tr')
                        .addClass('active');
                } else {
                    checkbox
                        .closest('tr')
                        .removeClass('active');
                }
                if (jQuery("tbody input:checked").length == jQuery("tbody input:checkbox").length)
                    jQuery('thead input:checkbox', table).prop("checked", true);
                else
                    jQuery('thead input:checkbox', table).prop("checked", false);
                scope.$applyAsync(scope.options.selections = [])
                jQuery("tbody input:checked").each(function () {
                    scope.$applyAsync(
                        scope.options.selections.push(JSON.parse(this.value))
                    );
                });
            };
        }
    };
});

HaselApp.directive('chatWindow', ['$timeout', function ($timeout) {
    return {
        scope: { ngModel: '=', options: '=' },
        link: function (scope, element) {
            var lWindow, lHeader, lFooter, cContainer, cHead, cTalk, cPeople, cform, cTimeout, chatInput;

            if (typeof scope.options == 'undefined')
                scope.options = {};
            if (typeof scope.options.MessageDirection == 'undefined')
                scope.options.MessageDirection = 'ascending';

            // Init chat
            var initChat = function () {
                // Set variables
                lWindow = angular.element(window);
                lHeader = angular.element('#header-navbar');
                lFooter = angular.element('#page-footer');
                cContainer = element;
                cHead = angular.element('.js-chat-head');
                cTalk = element.find('.js-chat-talk');
                cPeople = element.find('.js-chat-people');
                cform = element.find('.js-chat-form');
                chatInput = element.find('.js-chat-input');
                scope.WindowMaxHeight = 0;

                if (cContainer.data('chat-height'));
                scope.WindowMaxHeight = cContainer.data('chat-height');

                // Chat layout mode
                switch (cContainer.data('chat-mode')) {
                    case 'full':

                        // Init chat windows' height
                        initChatWindows();

                        // ..also on browser resize or orientation change
                        lWindow.on('resize orientationchange', function () {
                            clearTimeout(cTimeout);

                            cTimeout = setTimeout(function () {
                                initChatWindows();
                            }, 150);
                        });
                        break;
                    case 'fixed':

                        // Init chat windows' height with a specific height
                        initChatWindows(scope.WindowMaxHeight);
                        break;
                    case 'popup':

                        // Init chat windows' height with a specific height
                        initChatWindows(scope.WindowMaxHeight);

                        // Adjust chat container
                        cContainer.css({
                            'position': 'fixed',
                            'right': '10px',
                            'bottom': 0,
                            'display': 'inline-block',
                            'padding': 0,
                            'width': '70%',
                            'max-width': '420px',
                            'min-width': '300px',
                            'z-index': '1031'
                        });
                        break;
                    default:
                        return false;
                }

                // Enable scroll lock to chat talk window
                $(cTalk).scrollLock();
            };

            scope.options.ScrollBottom = function () {
                cTalk.slimScroll({ scrollTo: cTalk[0].scrollHeight });
            }

            // Init chat windows' height
            var initChatWindows = function (customHeight) {
                if (customHeight) {
                    cHeight = customHeight;
                } else {
                    // Calculate height
                    var cHeight = lWindow.height() -
                        lHeader.outerHeight() -
                        lFooter.outerHeight() -
                        cHead.outerHeight() -
                        (parseInt(cContainer.css('padding-top')) + parseInt(cContainer.css('padding-bottom')));

                    // Add a minimum height
                    if (cHeight < 200) {
                        cHeight = 200;
                    }
                }

                // Set height to chat windows (+ people window if exists)
                if (cPeople) {
                    cPeople.css('height', cHeight);
                }
                cTalk.css('max-height', cHeight - cform.outerHeight());
            };

            initChat();

            $timeout(function () {
                if (scope.options.MessageDirection === 'ascending')
                    scope.options.ScrollBottom();
            }, 200);

            // Add Message
            scope.options.AddMessage = function (chatMsgObj) {
                if (typeof scope.ngModel == 'undefined')
                    scope.ngModel = [];
                if (scope.options.MessageDirection == 'ascending')
                    $timeout(function () { scope.ngModel.push(chatMsgObj) });
                else if (scope.options.MessageDirection == 'descending')
                    $timeout(function () { scope.ngModel.unshift(chatMsgObj) });

                // If message and chat window exists
                if (chatMsgObj && cTalk.length) {
                    // Scroll the message list to the bottom
                    scope.options.ScrollBottom();

                    // If input is set, reset it
                    if (chatInput) {
                        chatInput.val('');
                    }
                }
            };

            if (typeof scope.options.IsFullScreen == 'undefined')
                scope.options.IsFullScreen = false;

            scope.$watch('options.IsFullScreen', function (e) {
                if (e) {
                    initChatWindows();
                } else {
                    initChatWindows(scope.WindowMaxHeight);
                }
            });
        }
    };
}]);

HaselApp.directive('slimScroll', function () {
    return {
        link: function (scope, element, attrs) {
            var options = (typeof scope.$eval(attrs.slimScroll) !== 'undefined') ? scope.$eval(attrs.slimScroll) : new Object();

            jQuery(element).slimScroll({
                height: options.height ? options.height : 'inhrerit',
                size: options.size ? options.size : '5px',
                position: options.position ? options.position : 'right',
                color: options.color ? options.color : '#000',
                alwaysVisible: options.alwaysVisible ? true : false,
                railVisible: options.railVisible ? true : false,
                railColor: options.railColor ? options.railColor : '#999',
                railOpacity: options.railOpacity ? options.railOpacity : .3
            });
        }
    };
});

HaselApp.directive('blur', function () {
    return {
        link: function (scope, element) {
            element.bind('click', function () {
                element.blur();
            });
        }
    };
});

HaselApp.directive('a', function () {
    return {
        restrict: 'E',
        link: function (scope, elem, attrs) {
            if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                elem.on('click', function (e) {
                    e.preventDefault(); // prevent link click for above criteria
                });
            }
        }
    };
});

// Handle Dropdown Hover Plugin Integration
HaselApp.directive('dropdownMenuHover', function () {
    return {
        link: function (scope, elem) {
            elem.dropdownHover();
        }
    };
});

HaselApp.directive('haselDataGrid', [
    function () {
        return {
            replace: true,
            transclude: true,
            restrict: 'EA',
            scope: {
                ngModel: '=',
                ngEnabled: '=',
                options: '=',
            },
            templateUrl: '/Content/HtmlTemplates/HaselDataGridTemplate.html?v=2',
            controller: [
                "$scope", "$attrs", "$element", "$timeout", "$notification", "$http", "$compile",
                function ($scope, $attrs, $element, $timeout, $notification, $http, $compile) {
                    if (typeof $scope.options === 'undefined')
                        $scope.options = {};

                    $timeout(function () {
                        $scope.options.Refresh = function () {
                            $scope.Grid = $($element).data("dxDataGrid");
                            $scope.Grid.refresh();
                        }
                    }, 400);

                    $scope.options.ExportToExcel = function () {
                        $scope.Grid.exportToExcel();
                    }

                    $scope.filterRow = {
                        visible: true,
                        applyFilter: "auto"
                    };
                    $scope.headerFilter = {
                        visible: true,
                    };
                    var store = null;

                    if (typeof $scope.options.itemAlias === 'undefined')
                        $scope.options.itemAlias = "entity";
                    if (typeof $scope.options.dataSourceUrl !== 'undefined') {
                        store = new DevExpress.data.CustomStore({
                            load: function (loadOptions) {
                                var deferred = $.Deferred();
                                if (typeof $scope.options.dataSourceFilter !== 'undefined')
                                    $http.post($scope.options.dataSourceUrl, $scope.options.dataSourceFilter).then(
                                        function (response) {
                                            var result = response.data;
                                            if (result.IsSuccess)
                                                deferred.resolve(result.Data);
                                        },
                                        function (e) {
                                            return e;
                                        });
                                else
                                    $http.post($scope.options.dataSourceUrl).then(
                                        function (response) {
                                            var result = response.data;
                                            if (result.IsSuccess)
                                                deferred.resolve(result.Data);
                                        },
                                        function (e) {
                                            return e;
                                        });
                                return deferred.promise();
                            }
                        });

                        $scope.options.dataSource = store;
                    }
                    if (typeof $scope.options.hoverStateEnabled === 'undefined')
                        $scope.options.hoverStateEnabled = true;
                    if (typeof $scope.options.selection === 'undefined')
                        $scope.options.selection = {
                            mode: "single"
                        };
                    if (typeof $scope.options.onSelectionChanged === 'undefined')
                        $scope.options.onSelectionChanged = function (selectedItems) {
                            var data = selectedItems.selectedRowsData[0];
                            if (data)
                                $scope.options.SelectedRow = data;
                        }

                    if (typeof $scope.options.bindingOptions === 'undefined')
                        $scope.options.bindingOptions = {
                            filterRow: "filterRow",
                            headerFilter: "headerFilter"
                        }
                    if (typeof $scope.options.ReportName === 'undefined')
                        $scope.options.ReportName = "GridReport" + " -" + moment().format('YYYYMMDDhhmmss');
                    if (typeof $scope.options.export === 'undefined')
                        $scope.options.export = {
                            enabled: false,
                            fileName: $scope.options.ReportName
                        }

                    if (typeof $scope.options.paging === 'undefined')
                        $scope.options.paging = {
                            pageSize: 10
                        }

                    if (typeof $scope.options.sorting === 'undefined')
                        $scope.options.sorting = {
                            mode: "multiple"
                        }
                    if (typeof $scope.options.pager === 'undefined')
                        $scope.options.pager = {
                            showPageSizeSelector: true,
                            allowedPageSizes: [5, 10, 20],
                            showInfo: true
                        }

                    if (typeof $scope.options.filterRow === 'undefined')
                        $scope.options.filterRow = {
                            visible: true,
                            applyFilter: "auto"
                        }

                    if (typeof $scope.options.allowColumnReordering === 'undefined')
                        $scope.options.allowColumnReordering = true;

                    if (typeof $scope.options.allowColumnResizing === 'undefined')
                        $scope.options.allowColumnResizing = true;

                    if (typeof $scope.options.columnAutoWidth === 'undefined')
                        $scope.options.columnAutoWidth = true;

                    if (typeof $scope.options.columnChooser === 'undefined')
                        $scope.options.columnChooser = {
                            enabled: false
                        };

                    if (typeof $scope.options.groupPanel === 'undefined')
                        $scope.options.groupPanel = {
                            visible: false
                        };
                    if (typeof $scope.options.columnFixing === 'undefined')
                        $scope.options.columnFixing = {
                            enabled: true
                        };

                    if (typeof $scope.options.columns === 'undefined')
                        console.log($scope.ngModel + " grid columns not found");
                }
            ]
        }
    }
]);
HaselApp.directive('holdMinNumber', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attrs, ngModel) {
            scope.ngModel = ngModel;
            scope.$watchCollection('ngModel', function () {
                if (parseInt(ngModel.$viewValue) < parseInt(attrs.holdMinNumber)) {
                    elem.val(parseInt(attrs.holdMinNumber));
                    ngModel.$setViewValue(parseInt(attrs.holdMinNumber))
                }
            });
        }
    };
});
HaselApp.directive('holdMaxNumber', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attrs, ngModel) {
            scope.ngModel = ngModel;
            scope.$watchCollection('ngModel', function () {
                if (parseInt(ngModel.$viewValue) > parseInt(attrs.holdMaxNumber)) {
                    elem.val(parseInt(attrs.holdMaxNumber));
                    ngModel.$setViewValue(parseInt(attrs.holdMaxNumber))
                }
            });
        }
    };
});
HaselApp.directive('messagePanel', ["$rootScope", "$timeout", function ($rootScope, $timeout) {
    return {
        restrict: 'E',
        scope: {
            ngModel: '='
        },
        replace: true,
        templateUrl: "/Content/HtmlTemplates/AlertPanelTemplate.html?v=2",
        link: function (scope, elem, attrs) {
            scope.$watch("ngModel",
                function (e) {
                    if (typeof e !== 'undefined') {
                        scope.result = e;
                        if (typeof scope.result != "undefined" && typeof scope.result.ResultType != null && typeof scope.result.ResultType != "undefined") {
                            if (scope.result.ResultType != 'Hide' && typeof elem.attr('Id') !== 'undefined')
                                $rootScope.ScrollTo(elem.attr('Id'));
                            if (scope.result.ResultType == "Success") {
                                $timeout(function () {
                                    scope.result.ResultType = "Hide";
                                }, 2000);
                            }
                        }
                    }
                });
        }
    };
}]);
HaselApp.directive('includeReplace', ['$timeout', function ($timeout) {
    return {
        require: 'ngInclude',
        restrict: 'A', /* optional */
        link: function (scope, el, attrs) {
            $timeout(function () {
                el.replaceWith(el.children());
            }, 200);
        }
    };
}]);
HaselApp.directive('includeExtract', ['$timeout', function ($timeout) {
    return {
        require: 'ngInclude',
        restrict: 'A', /* optional */
        scope: {
            includeExtract: '='
        },
        link: function (scope, el, attrs) {
            $timeout(function () {
                el.after(el.children());
                el.children().empty();
                el.html(scope.includeExtract);
            }, 300);
        }
    };
}]);
 HaselApp.directive('includeReplace', ['$timeout',function ($timeout) {
    return {
        require: 'ngInclude',
        restrict: 'A',
        scope:{
            includeReplace : '='
        },
        link: function (scope, el, attrs) {
            $timeout(function(){
            var optGroup = angular.element("<optGroup>");
            optGroup.attr("label", scope.includeReplace);
            optGroup.append(el.children());
            el.replaceWith(optGroup);
            },200);
        }
    };
}]);
 
HaselApp.directive('convertToNumber', function() {
    return {
        require: 'ngModel',
        link: function(scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function(val) {
                return parseInt(val, 10);
            });
            ngModel.$formatters.push(function(val) {
                return '' + val;
            });
        }
    };
});
HaselApp.directive('treeSelect', [
    function () {
        return {
            restrict: 'EA',
            scope: {
                options: '=',
                ngModel: '=',
                ngEnabled: '=',
                ngRequired: '='
               
            },
            templateUrl: '/Content/HtmlTemplates/TreeSelectTemplate.html?v=1',

            controller: ["$scope", "$attrs", "$element", "$timeout", "$notification", "$compile", function ($scope, $attrs, $element, $timeout, $notification, $compile) {
                if (typeof $scope.options === 'undefined')
                    $scope.options = {};
                if ($attrs.name !== 'undefined')
                    $scope.Name = $attrs.name;
                if ($scope.ngModel === 0)
                    $scope.ngModel = "";
                $scope.options.IsLoading = false;
                if (typeof $scope.options.AllowEmptyModel == 'undefined')
                    $scope.options.AllowEmptyModel = true;
                if (typeof $scope.options.placeHolder === 'undefined')
                    $scope.options.placeHolder = 'Seçiniz';
                
                var searchSelecModeltimer = null;
                $scope.$watch('ngModel', function () {
                    if ($scope.ngModel === 0)
                        $scope.ngModel = "";
                    if (typeof $scope.options.onChange == 'function') {
                        if (searchSelecModeltimer)
                            $timeout.cancel(searchSelecModeltimer);
                        if (typeof $scope.options.onChangeFilter != 'undefined')
                            searchSelecModeltimer = $timeout(function () {
                                $scope.options.onChange($scope.options.onChangeFilter);
                            }, 10);
                        else
                            $timeout(function () {
                                $scope.options.onChange($scope.ngModel);
                            }, 10);
                    }
                });

                if (typeof $scope.options.DataSource !== 'undefined' && typeof $scope.options.DataSource !== 'function') {
                    $scope.options.data = $scope.options.DataSource;
                }

                $scope.triggerDataSourceBound = function () {
                    if (typeof $scope.options.onDataBound == 'function')
                        $scope.options.onDataBound();
                }

                $scope.SuccessFn = function (result) {
                    $timeout(function () { $scope.options.IsLoading = false; });

                    if (result.IsSuccess) {
                        $scope.options.data = result.Data;
                    } else {
                        $scope.WarningMessage = result.Message == null ? "Tanımlanamayan bir hata oluştu" : result.Message;
                        $notification.Notify("Hata", $scope.WarningMessage, "d");
                    }
                    $scope.triggerDataSourceBound();
                }

                $scope.ErrorFn = function (result) {
                    $scope.options.IsLoading = false;
                    $notification.Notify("Hata", result.statusText, "d");
                    $scope.triggerDataSourceBound();
                }

                $scope.options.GetData = function () {
                    if (typeof $scope.options.DeferDataSource !== 'undefined')
                        $scope.options.DeferDataSource($scope.options.BindingParams).then($scope.SuccessFn);

                    if (typeof $scope.options.DataSource !== 'undefined' && typeof $scope.options.DataSource === 'function') {
                        $scope.options.IsLoading = true;
                        if (typeof $scope.options.SearchFilter != 'undefined')
                            $scope.options.DataSource($scope.options.BindingParams, angular.copy($scope.options.SearchFilter), $scope.SuccessFn, $scope.ErrorFn);
                        else
                            $scope.options.DataSource($scope.options.BindingParams, $scope.SuccessFn, $scope.ErrorFn);
                    }
                }

                if (typeof $scope.options.onLoad !== 'undefined' && typeof $scope.options.onLoad === 'function')
                    $scope.options.onLoad();

                if (typeof $scope.options.BindOnLoad === 'undefined' || $scope.options.BindOnLoad === true)
                    $scope.options.GetData();

                $scope.GetIndentText = function (text, level) {
                    var ret = "";
                    for (i = 0; i < level + 1 ; i++) {
                        ret += "&nbsp;";
                    }
                    ret += " " + text;
                    return ret;
                }
            }]
        }
    }
]);