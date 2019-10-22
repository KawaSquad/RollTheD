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
					$req->execute(array('loginPost' => $loginPost,'passwordPost' => $passwordPost));
					$req->closeCursor();

					$success = true;					
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
	
	function RequestSessionsList()
    {
        $jsonRequest = new JsonFormat();
        $success = false;
        
			if($this->ConnectToDB())
			{			
				$req = self::$bdd->prepare('SELECT * FROM t_Roll_Session');
				$req->execute();
				
				$results = $req->fetchall();
				$req->closeCursor();
                
				if($results)
				{
					$sessionsList = new SessionList();
					$sessionsList->sessions = $results;

                    $jsonRequest->content = json_encode($sessionsList);
					$success = true;
				}
				else
				{
                    $jsonRequest->error= "No sessions created yet";
				}
			}
			else
            {
                $jsonRequest->error= "Connection Fail";
			}
        
        $jsonRequest->success= $success;
		$jsonEncode =  (json_encode($jsonRequest));
        echo $jsonEncode;
	}
  
	function NewSession($jsonRequest)
    {
        $success = false;
		
		$nameSessionPost = "";
		$masterSessionPost = "";
		$ipSessionPost = "";

        if(isset($_POST['nameSessionPost']) && isset($_POST['masterSessionPost']) && isset($_POST['ipSessionPost']))
		{
            $nameSessionPost = $_POST['nameSessionPost'];
			$masterSessionPost = $_POST['masterSessionPost'];
			$ipSessionPost = $_POST['ipSessionPost'];
    
			if($this->ConnectToDB())
			{	
				//Check If exist 
				
				$req = self::$bdd->prepare('SELECT * FROM t_Roll_Session WHERE Name_Session = :nameSessionPost');
				$req->bindParam('nameSessionPost',$nameSessionPost);

				$req->execute();
				$result = $req->fetch();
				$req->closeCursor();
				
				$sessionClear = true;
				
				if($result)
				{
					$sessionClear = false;
					$jsonRequest->error= "Session already created";
				}

				//Create new
				if($sessionClear)
				{
					$req = self::$bdd->prepare('INSERT INTO t_Roll_Session ( Name_Session, Master_Session,IP_Session) VALUES (:nameSessionPost,:masterSessionPost,:ipSessionPost)');
	
					$req->execute(array('nameSessionPost' => $nameSessionPost,'masterSessionPost' => $masterSessionPost,'ipSessionPost' => $ipSessionPost));
					$result = $req->fetch();
					$req->closeCursor();

					$success = true;
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
		// Comment : Cause of the create folder
		// $jsonEncode =  (json_encode($jsonRequest));
        // echo $jsonEncode;
    }

	function RequestCharactersList()
    {
        $jsonRequest = new JsonFormat();
        $success = false;
		
		$idSessionPost = "";

        if(isset($_POST['SessionIDPost']))
		{

			$idSessionPost = $_POST['SessionIDPost'];
			
			if($this->ConnectToDB())
			{			
				$req = self::$bdd->prepare('SELECT * FROM t_Roll_Character WHERE ID_Session = :ID_SessionPost');
				$req->bindParam('ID_SessionPost',$idSessionPost);
				$req->execute();
				
				$result = $req->fetchall();
				$req->closeCursor();
                
				if($result)
				{
					$charactersList = new CharacterList();
					$charactersList->characters = $result;
                    $jsonRequest->content = json_encode($charactersList);
					$success = true;
				}
				else
				{
					$jsonRequest->error= "No characters created yet";
				}
			}
			else
            {
				$jsonRequest->error= "Connection Fail";
			}
		}

        $jsonRequest->success= $success;
		$jsonEncode =  (json_encode($jsonRequest));
        echo $jsonEncode;
	}

	function RequestClassesRacesList()
    {
		
		$jsonRequest = new JsonFormat();
        $success = false;

		if($this->ConnectToDB())
		{			
			$reqClasses = self::$bdd->prepare('SELECT Class FROM t_Roll_Classes ORDER BY Class ASC');
			$reqClasses->execute();
			$resultsClass = $reqClasses->fetchall();
			$reqClasses->closeCursor();
			
			$reqRaces = self::$bdd->prepare('SELECT Race FROM t_Roll_Races');
			$reqRaces->execute();
			$resultsRaces = $reqRaces->fetchall();
			$reqRaces->closeCursor();
			
			if($resultsClass && $resultsRaces)
			{
				$list = new ListParse();
				$list->classes = $resultsClass;
				$list->races = $resultsRaces;

				$jsonRequest->content = json_encode($list);
				if($jsonRequest->content)
				{
					$success = true;
				}
				else
				{
					$comptClasses = 0;
					foreach($resultsClass as $value)
					{
						$comptClasses++;
						$valueEncode = json_encode($value);
						if($valueEncode == false)
						{
							echo "Class : {$comptClasses} is not parsable";
						}
					}

					$comptRaces = 0;
					foreach($resultsRaces as $value)
					{
						$comptRaces++;
						$valueEncode = json_encode($value);
						if($valueEncode == false)
						{
							echo "Race : {$comptRaces} is not parsable";
						}
					}

					$jsonRequest->error= "UTF8 ERROR";
				}					
			}
			else
			{
				$jsonRequest->error= "No classes created yet";
			}
		}
		else
		{
			$jsonRequest->error= "Connection Fail";
		}
		$jsonRequest->success= $success;
		$jsonEncode =  (json_encode($jsonRequest));
		echo $jsonEncode;
		
	}

	function NewCharacter()
    {
    	$jsonRequest = new JsonFormat();
		$success = false;
		
		//Character
		$character_name_Post = "";
		$level_Post = "";
		$hp_Post = "";
		$hp_max_Post = "";
		$gold_Post = "";

		//Foreign key
		$class_Post = "";
		$race_Post = "";

		//Stats
		$stat_str_Post = "";
		$stat_dex_Post = "";
		$stat_int_Post = "";
		$stat_con_Post = "";
		$stat_wis_Post = "";
		$stat_cha_Post = "";

		try 
		{
            $character_name_Post = $_POST['character_name_Post'];
            $level_Post = $_POST['level_Post'];
            $hp_Post = $_POST['hp_Post'];
			$hp_max_Post = $_POST['hp_max_Post'];
			
			$class_Post = $_POST['class_Post'];
			$race_Post = $_POST['race_Post'];
			
            $stat_str_Post = $_POST['stat_str_Post'];
            $stat_dex_Post = $_POST['stat_dex_Post'];
            $stat_int_Post = $_POST['stat_int_Post'];
            $stat_con_Post = $_POST['stat_con_Post'];
            $stat_wis_Post = $_POST['stat_wis_Post'];
			$stat_cha_Post = $_POST['stat_cha_Post'];			

			if($this->ConnectToDB())
			{	
				//Check If exist 
				
				$req = self::$bdd->prepare('SELECT * FROM t_Roll_Character WHERE Character_Name = :characterNamePost');
				$req->bindParam('characterNamePost',$character_name_Post);

				$req->execute();
				$result = $req->fetch();
				$req->closeCursor();
				
				$characterClear = true;
				
				if($result)
				{
					$characterClear = false;
					$jsonRequest->error= "Character already exist";
				}

				//Create new
				if($characterClear)
				{
					// + HERE + //
					$req = self::$bdd->prepare('INSERT INTO t_Roll_Character ( Name_Session, Master_Session,IP_Session) VALUES (:nameSessionPost,:masterSessionPost,:ipSessionPost)');
	
					$req->execute(array('nameSessionPost' => $nameSessionPost,'masterSessionPost' => $masterSessionPost,'ipSessionPost' => $ipSessionPost));
					$result = $req->fetch();
					$req->closeCursor();

					$success = true;
				}   
			}
			else
            {
                $jsonRequest->error= "Connection Fail";
			}
		}
		catch (Exception $e)
		{
            $jsonRequest->error= "Post error";
		}
		$jsonRequest->success= $success;
		$jsonEncode =  (json_encode($jsonRequest));
        echo $jsonEncode;
    }

	function CreateFolder($jsonRequest)
	{
		$success = false;
		
		$masterSessionPost = "";

		if(isset($_POST['masterSessionPost']))
		{
			$masterSessionPost = $_POST['masterSessionPost'];		

			$config = require("../restricted/configDB.php");

			$ftp_server = $config["ftp_server"];
			$ftp_username=$config["ftp_username"];
			$ftp_userpass=$config["ftp_userpass"];
			
			$ftp_connection = ftp_connect($ftp_server)  
				  or die("Could not connect to $ftp_server"); 
			
		  	if($ftp_connection) 
			{ 
				$login = ftp_login($ftp_connection, $ftp_username, $ftp_userpass); 
			  	if($login)
			  	{ 
				  	$dir = "www/RollTheD/Sessions/$masterSessionPost";
				  	$dirSaves = "$dir/Saves";
					$dirMaps = "$dir/Maps";
					$dirPictures = "$dir/Pictures";
					  
					if (ftp_chdir($ftp_connection, $dir))
					{
						$success = true;
						$jsonRequest->content = "$dir - Directory exist";
					}
					else
					{ 
						if (ftp_mkdir($ftp_connection, $dir)) 
						{
							if (ftp_mkdir($ftp_connection, $dirSaves) && ftp_mkdir($ftp_connection, $dirMaps) && ftp_mkdir($ftp_connection, $dirPictures)) 
							{
								$success = true;
								$jsonRequest->content = "$dir - New directory created save";
							}
							else
							{
								$jsonRequest->error = "Can not create the subs directories";
							}	
						}
						else
						{
							$jsonRequest->error = "Can not create the directory";
						}
					}
			  	} 
			  	else 
			  	{ 
					$jsonRequest->error = "Login fail";
				} 
			
		  		// Closeing  connection 
				ftp_close($ftp_connection);
			}
		}
		else
		{
			$jsonRequest->error = "Error Post!"; 			
		}

		$jsonRequest->success= $success;
		// $jsonEncode =  (json_encode($jsonRequest));
        // echo $jsonEncode;
	}

	//Database
    
    function ConnectToDB()
	{
        try
		{
            $config = require("../restricted/configDB.php");
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