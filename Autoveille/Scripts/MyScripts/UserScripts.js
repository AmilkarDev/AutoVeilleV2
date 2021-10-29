$(document).ready(function () {
    var table = $('#usersTable').DataTable
        (
            {
                "columnDefs": [
                    //{
                    //    "title": "Id Evenement ",
                    //    "targets": 0
                    //},    
                    //{
                    //    "width": "5%",
                    //    "targets": [0],

                    //},
                    //{
                    //    "searchable": true,
                    //    "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
                    //},
                    {
                        "className": "text-center custom-middle-align",
                        "targets": [0, 1, 2, 3, 4, 5, 6]
                    },],
                "language":
                {
                    "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
                },
                "searching": true,
                "processing": true,
                "serverSide": true,
                "scrollX": "true",
                "orderCellsTop": true,
                "fixedHeader": true,
                "ajax":
                {
                    "url": "/Utilisateurs/GetData",
                    "type": "POST",
                    "dataType": "JSON"
                },
                "columns": [
                    {
                        "data": "UserID",
                        "name": "Id   Utilisateur"
                    },
                    {
                        "data": "UserName",
                        "title": "username"
                    },
                    {
                        "data": "FirstName",
                        "title": "prenom"                    },
                    {
                        "data": "LastName",
                        "title": "nom"
                    },
                    {
                        "data": "Email",
                        "title": "Email"
                    },
                    {
                        "data": "Langue",
                        "render": function (data, type, row) {
                            //console.log(data);
                            //return data;
                            if (data) {
                                return "Anglais";
                            }
                            else { 
                                return "Français";
                            }
                        }
                    },
                    {
                        "data": "Role",
                        "render": function (data, type, row) {
                            //console.log(data);
                            //return data
                            if (data == 0) {
                                return "Gestionnaire";
                            }
                            else if (data == 1) {
                                return "Consultant";
                            }
                            else if (data == 2) {
                                return "Concessionnaire";
                            }
                        }
                    }
                ]

            });

});