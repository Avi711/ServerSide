$(function () {
    $('#search-form').submit(async e => {
        e.preventDefault();


        const q = $('#search').val();


        const response = await fetch('/Ratings/Search?query=' + q);
        console.log(response);
        const d = await response.json();
        console.log(d);

        $('#rateList').html('');

        const template = $('#template').html();
        let results = '';
        for (var item in d) {
            let row = template;
            for (var key in d[item]) {
                row = row.replaceAll('{' + key + '}', d[item][key]);
                row = row.replaceAll('%7B' + key + '%7D', d[item][key]);
            }
            results += row;
        }
        console.log(results);
        $('#rateList').html(results);
    })
});