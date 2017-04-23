var url = "http://iassetweatherapi.azurewebsites.net/api/weather/";

function popolateCities() {http://localhost:53027/fonts
    $("#cities").empty();
    $.ajax({
        url: url + $('#countryName').val() + "/cities",
        datatype: 'json',
        type: "get",
        success: function (data) {
           var response = data.cities;
            $.each(response, function (key, value) {
                $("#cities").append($("<option></option>").val(value.cityName).html(value.cityName));
            });
        },
        error: function () {
            $('#errorMessage').show();
            $('#tblWeather').hide();
        }
    });
};

function ShowWeather() {
    var city = $('#cities').val();
    if (city == null) {
        $('#errorMessage').show();
        $('#tblWeather').hide();
        return;
    }
    $.ajax({
        url: url + $('#countryName').val() + "/" + $('#cities').val(),
        datatype: 'json',
        type: "get",
        success: function (data) {
            $('#errorMessage').hide();
            $('#tblWeather').show();
            $('#Location').text(data.location);
            $('#Time').text(data.time);
            $('#Wind').text(data.wind);
            $('#Visibility').text(data.visibility);
            $('#SkyConditions').text(data.skyConditions);
            $('#Temperature').text(data.temperature);
            $('#DewPoint').text(data.dewPoint);
            $('#RelativeHumidity').text(data.relativeHumidity);
            $('#Pressure').text(data.pressure);
        },
        error: function () {
            $('#tblWeather').hide();
            $('#errorMessage').show();
        }
    });
}
       
