﻿'use strict';

angular.module('notifications', [])
    .factory('$notification', ['$timeout', function ($timeout) {

        //console.log('notification service online');
        var notifications = JSON.parse(localStorage.getItem('$notifications')) || [],
            queue = [];

        var settings = {
            info: { duration: 6000, enabled: false },
            warning: { duration: 6000, enabled: true },
            danger: { duration: 0, enabled: true },
            success: { duration: 6000, enabled: true },
            progress: { duration: 0, enabled: true },
            custom: { duration: 0, enabled: true },
            details: true,
            localStorage: false,
            html5Mode: false,
            html5DefaultIcon: 'icon.png'
        };

        function html5Notify(icon, title, content, ondisplay, onclose) {
            if (window.webkitNotifications.checkPermission() === 0) {
                if (!icon) {
                    icon = 'favicon.ico';
                }
                var noti = window.webkitNotifications.createNotification(icon, title, content);
                if (typeof ondisplay === 'function') {
                    noti.ondisplay = ondisplay;
                }
                if (typeof onclose === 'function') {
                    noti.onclose = onclose;
                }
                noti.show();
            }
            else {
                settings.html5Mode = false;
            }
        }

        return {

            /* ========== SETTINGS RELATED METHODS =============*/
            disableHtml5Mode: function () {
                settings.html5Mode = false;
            },

            disableType: function (notificationType) {
                settings[notificationType].enabled = false;
            },

            enableHtml5Mode: function () {
                // settings.html5Mode = true;
                settings.html5Mode = this.requestHtml5ModePermissions();
            },

            enableType: function (notificationType) {
                settings[notificationType].enabled = true;
            },

            getSettings: function () {
                return settings;
            },

            toggleType: function (notificationType) {
                settings[notificationType].enabled = !settings[notificationType].enabled;
            },

            toggleHtml5Mode: function () {
                settings.html5Mode = !settings.html5Mode;
            },

            requestHtml5ModePermissions: function () {
                if (window.webkitNotifications) {
                    //console.log('notifications are available');
                    if (window.webkitNotifications.checkPermission() === 0) {
                        return true;
                    }
                    else {
                        window.webkitNotifications.requestPermission(function () {
                            if (window.webkitNotifications.checkPermission() === 0) {
                                settings.html5Mode = true;
                            }
                            else {
                                settings.html5Mode = false;
                            }
                        });
                        return false;
                    }
                }
                else {
                    // console.log('notifications are not supported');
                    return false;
                }
            },

            /* ============ QUERYING RELATED METHODS ============ */

            getAll: function () {
                // Returns all notifications that are currently stored
                return notifications;
            },

            getQueue: function () {
                return queue;
            },

            /* ============== NOTIFICATION METHODS ============== */

            info: function (title, content, userData) {
                //console.log(title, content);
                return this.awesomeNotify('info', 'info-circle', title, content, userData);
            },

            danger: function (title, content, userData) {
                return this.awesomeNotify('danger', 'ban', title, content, userData);
            },

            success: function (title, content, userData) {
                return this.awesomeNotify('success', 'check', title, content, userData);
            },

            warning: function (title, content, userData) {
                return this.awesomeNotify('warning', 'warning', title, content, userData);
            },

            awesomeNotify: function (type, icon, title, content, userData) {
                /**
                 * Supposed to wrap the makeNotification method for drawing icons using font-awesome
                 * rather than an image.
                 *
                 * Need to find out how I'm going to make the API take either an image
                 * resource, or a font-awesome icon and then display either of them.
                 * Also should probably provide some bits of color, could do the coloring
                 * through classes.
                 */
                // image = '<i class="fa fa-' + image + '"></i>';
                return this.makeNotification(type, false, icon, title, content, userData);
            },

            notify: function (image, title, content, userData) {
                // Wraps the makeNotification method for displaying notifications with images
                // rather than icons
                return this.makeNotification('info', image, true, title, content, userData);
            },

            /** Notifications ***/
            Notify: function (title, content, type) {
                console.log(title, content, type);
                switch (type) {
                    case "s":
                        return this.awesomeNotify('success', 'check', title, content, "");
                    case "d":
                        return this.awesomeNotify('danger', 'ban', title, content, "");
                    case "i":
                        return this.awesomeNotify('info', 'info-circle', title, content, "");
                    case "w":
                        return this.awesomeNotify('warning', 'warning', title, content, "");
                    default:
                        enableHtml5Mode();
                        notify("/favicon.ico", title, content); break;
                }
            },

            makeNotification: function (type, image, icon, title, content, userData) {
                var notification = {
                    'type': type,
                    'image': image,
                    'icon': icon,
                    'title': title,
                    'content': content,
                    'timestamp': +new Date(),
                    'userData': userData
                };
                notifications.push(notification);

                if (settings.html5Mode) {
                    html5Notify(image, title, content, function () {
                        //console.log("inner on display function");
                    }, function () {
                        //console.log("inner on close function");
                    });
                }
                else {
                    queue.push(notification);
                    if (settings[type].duration > 0) {
                        $timeout(function removeFromQueueTimeout() {
                            queue.splice(queue.indexOf(notification), 1);
                        }, settings[type].duration);
                    }
                }

                this.save();
                return notification;
            },

            /* ============ PERSISTENCE METHODS ============ */
            save: function () {
                // Save all the notifications into localStorage
                // console.log(JSON);
                if (settings.localStorage) {
                    localStorage.setItem('$notifications', JSON.stringify(notifications));
                }
                // console.log(localStorage.getItem('$notifications'));
            },

            restore: function () {
                // Load all notifications from localStorage
            },

            clear: function () {
                notifications = [];
                this.save();
            }

        };
    }])
    .directive('notifications', ['$notification', '$compile', function ($notification, $compile) {

        var html =
            '<div class="dr-notification-wrapper" ng-repeat="noti in queue">' +
                '<div class="alert alert-{{noti.type}} alert-dismissable" >' +
                    '<i class="fa fa-{{noti.icon}} fa-2x pull-left"></i>' +
                    '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                    '<strong ng-show="noti.title">{{noti.title}}<br /></strong> ' +
                    '{{ noti.content }}' +
                '</div>' +
            '</div>';

        function link(scope, element, attrs) {
            var position = attrs.notifications;
            position = position.split(' ');
            element.addClass('dr-notification-container');
            for (var i = 0; i < position.length ; i++) {
                element.addClass(position[i]);
            }
        }

        return {
            restrict: 'A',
            scope: {},
            template: html,
            link: link,
            controller: ['$scope', function NotificationsCtrl($scope) {
                $scope.queue = $notification.getQueue();
                $scope.removeNotification = function (noti) {
                    $scope.queue.splice($scope.queue.indexOf(noti), 1);
                };
            }]

        };
    }]);