<?php
include ('Classes.php');

class RequestManager
{
	private static $bdd = null;
    private static $reporter = null;

    function RequestLogin()
    {
        $jsonRequest = new JsonFormat();
        $success = false;
        
        $loginPost = "";
		$passwordPost = "";

        if(isset($_POST['loginPost']) && isset($_POST['passwordPost']))
		{
            $loginPost = $_POST['loginPost'];
			$passwordPost = $_POST['passwordPost'];
    
			if($this->ConnectToDB())
			{			
                $req = self::$bdd->prepare('SELECT * FROM t_Roll_Account WHERE Name_Account = :loginPost AND Password_Account = :passwordPost');
                // $req = self::$bdd->prepare("SELECT * FROM t_Roll_Account WHERE Name_Account = 'styven184' AND Password = '123456'");
                
				$req->bindParam('loginPost',$loginPost);
				$req->bindParam('passwordPost',$passwordPost);
				
				$req->execute();

				$result = $req->fetch();
				$req->closeCursor();
                
				if($result)
				{
                    $jsonRequest->content = json_encode($result);
					$success = true;
				}
				else
				{
                    $jsonRequest->error= "Login don't exist";
				}
			}
			else
            {
                $jsonRequest->error= "Connection Fail";
			}
        }
        else
		{
            $jsonRequest->error= "Post error";
        }
        
        $jsonRequest->success= $success;
		$jsonEncode =  (json_encode($jsonRequest));
        echo $jsonEncode;
    }
    
    function NewAccount()
    {
        $jsonRequest = new JsonFormat();
        $success = false;
        
        $loginPost = "";
		$passwordPost = "";


        if(isset($_POST['loginPost']) && isset($_POST['passwordPost']))
		{
            $loginPost = $_POST['loginPost'];
			$passwordPost = $_POST['passwordPost'];
    
			if($this->ConnectToDB())
			{	
				//Check If exist 

				$req = self::$bdd->prepare('SELECT * FROM t_Roll_Account WHERE Name_Account = :loginPost');
                
				$req->bindParam('loginPost',$loginPost);
				$req->execute();
				$result = $req->fetch();
				$req->closeCursor();
				
				$loginClear = true;
				
				if($result)
				{
					$loginClear = false;
					$jsonRequest->error= "Login already exist";
				}

				//Create new
				if($loginClear)
				{
					$req = self::$bdd->prepare('INSERT INTO t_Roll_Account ( Name_Account, Password_Account) VALUES (:loginPost,:passwordPost)');
					$req->bindParam('loginPost',$loginPost);
					$req->bindParam('passwordPost',$passwordPost);

					$req->execute(array('loginPost' => $loginPost,'passwordPost' => $passwordPost));
					$req->closeCursor();

					$this->RequestLogin();
					return;
				}   
			}
			else
            {
                $jsonRequest->error= "Connection Fail";
			}
        }
        else
		{
            $jsonRequest->error= "Post error";
        }
        
        $jsonRequest->success= $success;
		$jsonEncode =  (json_encode($jsonRequest));
        echo $jsonEncode;
    }
    
    //Database
    
    function ConnectToDB()
	{
        try
		{
            $config = require("restricted/configDB.php");
			self::$bdd = new PDO($config["db"], $config["username"], $config["password"]);
			return true;
		}
		catch (PDOException $e)
		{
			return false;
		}
	}
}
?>