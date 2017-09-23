HaselApp.service("InterviewService", ["$http", "$rootScope", "$q",
    function ($http, $rootScope, $q) {
        this.GetInterview = function (filter, success, error) {
            return $rootScope.AjaxPost('/Interview/GetInterview');
        }

        this.GetAuthenticator = function (filter, success, error) {
    
            var id = parseInt(filter.filter);
            return $rootScope.AjaxPost('/Interview/GetAuthenticator', {filter :id});
        }

        this.GetInterviewImportant = function (filter, success, error) {
            return $rootScope.AjaxPost('/Interview/GetInterviewImportant');
        }


        this.GetInterviewUser = function (filter, success, error) {
            return $rootScope.AjaxPost('/Interview/GetInterviewUser');
        }
    }
]);