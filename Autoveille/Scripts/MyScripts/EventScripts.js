$(document).ready(function () {
    $('#TableId thead tr')
        .clone(true)
        .appendTo('#TableId thead');

    $('#TableId thead tr:eq(1) th').each(function (i) {
        if (i == 12) {
            var selectList = 

                '<select name="activ" id="active">' +
                '<option value="actif">Actif</option>' +
                '<option value="inactif">Inactif</option>' +
                '</select>';

            $(this).html(selectList);
        }
        else {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title.trim() + '" />');
        }

    });

   
    var table = $('#TableId').DataTable
        (
            {                
                "columnDefs": [
                {
                        "title": "Id Evenement ",
                    "targets": 0
                    },

                {
                    "width": "5%",
                    "targets": [0],
                    
                },
                {
                    "searchable": true,
                    "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,12]
                },
                {
                    "className": "text-center custom-middle-align",
                    "targets": [0, 1, 2, 3, 4, 5, 6,7 ,8, 9, 10, 11,12]
                },],
            "language":
            {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "searching" : true,
            "processing": true,
            "serverSide": true,
            "scrollX": "true",
            "orderCellsTop": true,
            "fixedHeader": true,
            "ajax":
            {
                "url": "/Evenements/GetData",
                "type": "POST",
                "dataType": "JSON"
            },
                "columns": [
                
                {
                    "data": "IdEvenement", "name": "Id   Evenement"
                },
                {
                    "data": "DateEvenementDebut",
                    "title": "Date Debut",
                    "type": "date",
                    "render": convertDate
                },
                {
                    "data": "DateEvenementFin",
                    "title": "Date Fin",
                    "render": convertDate
                },
                {
                    "data": "TotalEvenements"
                },
                {
                    "data": "DateCreation",
                    "render": convertDate
                },
                {
                    "data": "Utilisateur",
                },
                {
                    "data": "NoCommerce"
                },
                {
                    "data": "AppelsPrevusDirEv"
                },
                {
                    "data": "AppelsPrevusCASuly"
                },
                {
                    "data": "DateModification",
                    "render": convertDate
                },
                {
                    "data": "UtilisateurModification"
                },
                {
                    "data": "DatesConfirmer"
                },
                {
                    "data": "Actif",
                    "render": function (data, type, row) {
                        if (data) {
                            return "Actif";
                        }
                        else {
                            return "Inactif";
                        }
                    }
                }
                ]
             
        });

    table.columns().every(function (index) {
        $('.dataTable thead tr:eq(1) th:eq(' + index + ') input').on('keyup change', function () {
            table.column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    });


    $('.dataTable thead tr:eq(1) th:eq(' + 12 + ') select').on('keyup change', function () {
        table.column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });



    function convertDate(data) {
        if (data == null) return "";
        var s = data.substr(6, data.length - 8);
        var date = new Date(parseInt(s));
        var month = date.getMonth() + 1;
         return date.getDate() + "-" + (month.toString().length > 1 ? month : "0" + month) + "-" + date.getFullYear();
    }


    $('#TableId tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#buttonSupp').click(function () {
        var event = table.row('.selected').data();
        if (event) {

            event.DateCreation = convertDate(event.DateCreation);
            event.DateEvenementDebut = convertDate(event.DateEvenementDebut);
            event.DateEvenementFin = convertDate(event.DateEvenementFin);
            event.DateModification = convertDate(event.DateModification);
            $.ajax({
                url: "../Evenements/SupprimerEvenement",
                contentType: 'application/json; charset=utf-8',
                type: "POST",
                dataType: 'json',
                data: JSON.stringify(event),
                success: function (response) {
                    $('.modal-body').html(response);
                    $('#eventModal').modal("show");
                    $('#TableId').DataTable().ajax.reload();
                },
                error: function (xhr) {
                    alert("Problème de suppression d'évènements");
                }
            });

        }
        else {
            $('.modal-body').html("Veuillez sélectionner un évènement à supprimer");
            $('.modal-body').modal("show");
        }
    });

    $('#buttonEdit').click(function () {
        var event = table.row('.selected').data();
        if (event) {
            $.ajax({
                url: "../Evenements/ModifierEvenement",
                type: "get", 
                data: { IdEvenement: event.IdEvenement },
                success: function (response) {
                    $('.modal-body').html(response);
                    $('.modal-body').modal("show");
                },
                error: function (xhr) {
                    console.log("hahahahaha");
                }
            });
        }
        else {
            $('.modal-body').html("Veuillez sélectionner un évènement à modifier");
            $('.modal-body').modal("show");
        }
    });

    $('#buttonAdd').click(function () {
        $.ajax({
            url: "../Evenements/AjouterEvenement",
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

