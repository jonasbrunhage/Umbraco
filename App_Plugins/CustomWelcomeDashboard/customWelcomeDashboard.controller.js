﻿angular.module("umbraco").controller("CustomWelcomeDashboardController", function ($scope, userService, logResource, entityResource) {
    var vm = this;
    vm.UserName = "guest";
    vm.UserLogHistory = [];

    var user = userService.getCurrentUser().then(function (user) {
        console.log(user);
        vm.UserName = user.name;
    });

    var userLogOptions = {
        pageSize: 10,
        pageNumber: 1,
        orderDirection: "Descending",
        sinceDate: new Date(2020, 1, 1)
    };

    logResource.getPagedUserLog(userLogOptions)
        .then(function (response) {
            console.log(response);
            vm.UserLogHistory = response;
            var filteredLogEntries = [];

            angular.forEach(response.items, function (item) {
                if (item.nodeId > 0) {
                    var nodesWeKnowAbout = [];
                    if (nodesWeKnowAbout.indexOf(item.nodeId) !== -1)
                        return;

                    if (item.entityType === "Template") {
                        return;
                        }

                    if (item.logType === "Save" || item.logType === "SaveVariant") {

                        if (item.entityType === "Document") {
                            item.editUrl = "content/content/edit/" + item.nodeId;
                        }
                        if (item.entityType === "Media") {
                            item.editUrl = "media/media/edit/" + item.nodeId;
                        }
                        if (typeof item.entityType !== 'undefined') {
                            var ent = entityResource.getById(item.nodeId, item.entityType).then(function (ent) {
                                console.log(ent);
                                item.Content = ent;
                            });

                            nodesWeKnowAbout.push(ent.id);
                            filteredLogEntries.push(item);
                        }
                    }
                }
            });
            vm.UserLogHistory.items = filteredLogEntries;
        });
});

