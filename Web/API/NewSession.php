
<?php
    header('Content-Type: text/html; charset=utf-8');

    include ('RequestManager.php');
    
    $request = new RequestManager();
    $jsonRequest = new JsonFormat();
    
    $request->CreateFolder($jsonRequest);
    if($jsonRequest->success)
    {
        $jsonRequest = new JsonFormat();
        $request->NewSession($jsonRequest);
    }

    $jsonEncode =  (json_encode($jsonRequest));
    echo $jsonEncode;
?> 
