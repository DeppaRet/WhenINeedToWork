//import * as additionalMethods from "../lib/jquery-validation/dist/additional-methods"

const modal = $.modal({
    title: 'Authorization',
    closable: true,
    content: `
    <p>Логин</p> 
    <input id="email" type="email" name="login" asp-for="email" placeholder="Komm.mm@yandex.ru">
    <p>Пароль</p> 
    <input id="password" type="password" name = "password" asp-for="password" pattern="(?=.*\d)(?=.*[a-zA-Zа-яА-Я]).{6,}" title="Не менее 6 знаков, в том числе хотя бы одна цифра и буква" >`,
    width: '700 px',
    footerButtons: [
        {
            text: 'Принять', type: 'primary', handler() {
                console.log('primary btn clicked')
                let email = document.getElementById('email').value
                let password = document.getElementById('password').value // получаем по id значение 
                console.log(email)
                console.log(password)
                modal.close() //обращение к функции в другом файле
            }
        },
        {
            text: 'Отмена', type: 'danger', handler() {
                console.log('danger btn clicked')
                modal.close()
            }
        }
    ]   
})



document.addEventListener('click', event => {
    const btnType = event.target.dataset.btn
    //event.preventDefault()                       // блочит все ссылки

    if (event.target.dataset.btn === 'login') {
        modal.open()
    }
});

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

document.getElementById('calculateSalary').addEventListener('click', calculate);



