const modal = $.modal({
    title: 'Authorization',
    closable: true,
    content: `
    <p>Логин</p> 
    <input type="email" name="login">
    <p>Пароль</p> 
    <input type="password" name = "password">
    <input type="submit" name="submit" title="Отправить">
    <p>Text</p>`,
    width: '700 px',
    footerButtons: [
        {
            text: 'Ок', type: 'primary', handler() {
                console.log('primary btn clicked')
                modal.close()  //обращение к функции в дургом файле
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
    event.preventDefault()

    if (event.target.dataset.btn === 'login') {
        modal.open()
    }
});

