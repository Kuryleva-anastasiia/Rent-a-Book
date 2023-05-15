<?php
// Файлы phpmailer
require 'phpMailer/PHPMailer.php';
require 'phpMailer/SMTP.php';
require 'phpMailer/Exception.php';

// Переменные, которые отправляет пользователь
$name = $_POST['name'];
$phone = $_POST['phone'];
// $message = $_POST['message'];
$email = $_POST['email'];

// Формирование самого письма
// footer form
// $title = "Новое обращение Best Tour Plan";
// $body = "
// <h2>Новое обращение</h2>
// <b>Имя:</b> $name<br>
// <b>Телефон:</b>$phone<br><br>
// <b>Сообщение:</b><br>$message
// ";
// newsletter form
$email_title = "Подписка на рассылку";
$email_body = "
<h2>Новый запрос на рассылку</h2>
<b>Почта:</b><br>$email
";
// view other options modal form
$modal_title = "Новое обращение Bookstore";
$modal_body = "
<h2>Новое обращение: </h2>
<b>Имя:</b>$name<br>
<b>Телефон:</b>$phone<br>
<b>Почта:</b>$email<br>
";

// Настройки PHPMailer
$mail = new PHPMailer\PHPMailer\PHPMailer();

try {
    $mail->isSMTP();   
    $mail->CharSet = "UTF-8";
    $mail->SMTPAuth   = true;
    $mail->SMTPDebug = 2;
    $mail->Debugoutput = function($str, $level) {$GLOBALS['status'][] = $str;};

    // Настройки вашей почты
    $mail->Host       = 'smtp.mail.ru'; // SMTP сервера вашей почты
    $mail->Username   = 'knastya76r@mail.ru'; // Логин на почте
    $mail->Password   = '7irHfRY5z1ro393yQehh'; // Пароль на почте
    $mail->SMTPSecure = 'ssl';
    $mail->Port       = 465;
    $mail->setFrom('knastya76r@mail.ru', 'Pool of Books'); // Адрес самой почты и имя отправителя

    // Получатель письма
    $mail->addAddress('knastya76@yandex.ru');

// Отправка сообщения
$mail->isHTML(true);
// Проверка есть email или нет
if(isset($_POST['subscribe-btn'])){
    $mail->Subject = $email_title;
    $mail->Body = $email_body; 
    header('Location: ~/Shared/subscribe.html');
}
// if(isset($_POST['contact_button'])) {
//     $mail->Subject = $title;
//     $mail->Body = $body; 
//     header('Location: thankyou.html');
// }
if(isset($_POST['options_button'])) {
    $mail->Subject = $modal_title;
    $mail->Body = $modal_body; 
    header('Location: thankyou.html');
}


// Проверяем отравленность сообщения
if ($mail->send()) {$result = "success";} 
else {$result = "error";}

} catch (Exception $e) {
    $result = "error";
    $status = "Сообщение не было отправлено. Причина ошибки: {$mail->ErrorInfo}";
}
