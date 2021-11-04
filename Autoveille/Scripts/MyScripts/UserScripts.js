$(document).ready(function () {
    var table = $('#usersTable').DataTable
        (
            {
                "columnDefs": [
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
        if (user) {

            $.ajax({
                url: "../Utilisateurs/SupprimerUtilisateur",
                contentType: 'application/json; charset=utf-8',
                type: "POST",
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

        }
        else {
            $('.modal-body').html("Veuillez sélectionner un utilisateur à supprimer");
            $('.modal-body').modal("show");
        }
    });

    $('#buttonEdit').click(function () {
        var user = table.row('.selected').data();
        if (user) {
            $.ajax({
                url: "../Utilisateurs/ModifierUtilisateur",
                type: "get",
                data: { UserID: user.UserID },
                success: function (response) {
                    $('.modal-body').html(response);
                    $('.modal-body').modal("show");
                },
                error: function (xhr) {
                }
            });
        }
        else {
            $('.modal-body').html("Veuillez sélectionner un utilisateur à modifier");
            $('.modal-body').modal("show");
        }
       
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