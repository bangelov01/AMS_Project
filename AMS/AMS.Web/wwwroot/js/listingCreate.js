$(document).ready(function () {

    let type = $('#TypeId');
    let make = $('#MakeId');

    type.on('change', function () {

        let typeId = this.value;

        $('#ModelId').empty();

        let get = $.ajax({
            url: '/Listings/GetMakes',
            type: "get",
            data: {
                typeId: typeId
            }
        });

        get.done(function (response) {

            $('#MakeId').empty();

            for (key in response) {

                let el = document.createElement('option');
                el.value = response[key]['id'];
                el.textContent = response[key]['name'];

                $('#MakeId').append(el);
            }

            updateModels(typeId, response[0]['id'])
        });

        get.fail(function () {
            alert('Something went wrong!');
        })
    })


    make.on('change', function () {

        let typeId = $('#TypeId option:selected').val();

        updateModels(typeId, this.value);

    });

    function updateModels(typeId, makeId) {

        let get = $.ajax({
            url: '/Listings/GetModels',
            type: "get",
            data: {
                typeId: typeId,
                makeId: makeId
            }
        });

        get.done(function (response) {

            $('#ModelId').empty();

            for (key in response) {

                let el = document.createElement('option');
                el.value = response[key]['id'];
                el.textContent = response[key]['name'];

                $('#ModelId').append(el);
            }
        });

        get.fail(function () {
            alert('Something went wrong!');
        });
    }
});
