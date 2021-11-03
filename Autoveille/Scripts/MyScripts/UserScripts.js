﻿$(document).ready(function () {
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
                    },
                ],
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
                        "title": "Identificateur",
                        "name": "Id   Utilisateur"
                    },
                    {
                        "data": "UserName",
                        "title": "username"
                    },
                    {
                        "data": "FirstName",
                        "title": "prenom"
                    },
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
                        "title": "Langue",
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
                        "title": "Role",
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

    $('#usersTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#buttonSupp').click(function () {
        var user = table.row('.selected').data();
        console.log(user);
        $.ajax({
            url: "../Utilisateurs/SupprimerUtilisateur",
            contentType: 'application/json; charset=utf-8',
            type: "POST", //send it through get method
            dataType: 'json',
            data: JSON.stringify(user),
            success: function (response) {
                $('.modal-body').html(response);
                $('#userModal').modal("show");
                $('#usersTable').DataTable().ajax.reload();
            },
            error: function (xhr) {
                alert("Problème de suppression de l'utilisateur");
            }
        });
    });

    $('#buttonEdit').click(function () {
        var user = table.row('.selected').data();
        console.log(user);
        $.ajax({
            url: "../Utilisateurs/ModifierUtilisateur",
            type: "get", //send it through get method
            data: { UserID: user.UserID },
            success: function (response) {
                //console.log("rrrrrrrr");
                $('.modal-body').html(response);
                $('.modal-body').modal("show");
            },
            error: function (xhr) {
            }
        });
    });

    $('#buttonAdd').click(function () {
        $.ajax({
            url: "../Utilisateurs/AjouterUtilisateur",
            contentType: 'application/html; charset=utf-8',
            type: "get",
            dataType: 'html',
            success: function (response) {
                $('.modal-body').html(response);
                $('.modal-body').modal("show");
            },
            error: function (xhr) {
            }
        });
    });
});