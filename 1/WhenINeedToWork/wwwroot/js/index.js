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
            text: 'Ok', type: 'primary', handler() {
                console.log('primary btn clicked')
            }
        },
        {
            text: 'Cancel', type: 'danger', handler() {
                console.log('danger btn clicked')
            }
        }
    ]
})