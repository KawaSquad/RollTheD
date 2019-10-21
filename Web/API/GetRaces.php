
<?php
    header('Content-Type: text/html; charset=utf-8');

    include ('RequestManager.php');

    $request = new RequestManager();
    $request->RequestRacesList();
?> 
