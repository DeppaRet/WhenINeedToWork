//import * as additionalMethods from "../lib/jquery-validation/dist/additional-methods"


function calculate() {
    var calculated = document.getElementById('hours').value * document.getElementById('payment').value;
    var html;
    if (document.getElementById('salaryType').value === 'С вычетом налогов') {
        calculated = calculated * 0.87;
    }
    html = 'Рассчитанная ЗП: ' + calculated;
    console.log(calculated);
    document.getElementById('result').innerHTML = html;

}





