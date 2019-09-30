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
				$req = self::$bdd->prepare('SELECT User_ID FROM t_KawaWorld_User WHERE Login = :loginPost AND Password = :passwordPost');
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
	
	function RequestCharacter()
	{
		$jsonRequest = new JsonFormat();
		$success = false;

		$loginPost = "";
		
		if(isset($_POST['loginPost']))
		{
			$loginPost = $_POST['loginPost'];

			if($this->ConnectToDB())
			{			
				// $req = self::$bdd->prepare('SELECT * FROM t_KawaWorld_User AS u WHERE u.login = :login AND u.password = :password');

				$req = self::$bdd->prepare('SELECT c.Character_Name, c.url_picture  FROM t_KawaWorld_Character AS c INNER JOIN t_KawaWorld_User AS u ON (u.User_ID = c.User_ID) WHERE u.Login = :login ');
				$req->bindParam('login',$loginPost);
				
				$req->execute();
				
				$results = $req->fetchAll();
				$req->closeCursor();
				
				$characters = new Characters();
				$characters->characterList = array();

				foreach($results as $result)
				{
					$character = new Character();
					$character->character_name = $result['Character_Name'];
					$character->user_login = $loginPost;
					$character->url_picture = $result['url_picture'];
					
					$characters->characterList[] = $character;
				}

				$jsonRequest->content =  (json_encode($characters));
				$success = true;
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
		

	function NewUser()
	{
		$jsonRequest = new JsonFormat();
		$success = false;

		$loginPost;
		$emailPost;
		$passwordPost;

		if(isset($_POST['loginPost']) && isset($_POST['emailPost']) && isset($_POST['passwordPost']))
		{
			$loginPost = $_POST['loginPost'];
			$emailPost = strtolower($_POST['emailPost']);
			$passwordPost = $_POST['passwordPost'];
			
			if($this->ConnectToDB())
			{
				if($this->MailIsValid($emailPost))
				{
					if (!$this->MailExist($emailPost))
					{
						if (!$this->LoginExist($loginPost))
						{
							$req = self::$bdd->prepare('INSERT INTO t_KawaWorld_User(Login,Email,Password) VALUES(:login, :email, :password)');
							$req->execute(array('login' => $loginPost,'email' => $emailPost,'password' => $passwordPost));
							$req->closeCursor();
							
							$success = true;	
						}
						else
						{
							$jsonRequest->error= "Login already exist";
						}	
					}
					else
					{
						$jsonRequest->error= "Mail already exist";
					}
				}
				else
				{
					$jsonRequest->error= "Mail invalide";
				}
			}	
			else
			{
				$jsonRequest->error= "Connection error";
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


	function NewCharcter()
	{
		$jsonRequest = new JsonFormat();
		$success = false;

		$user_IDPost = 0;
		$characterNamePost = "";
		$url_Picture = "";

		if(isset($_POST['userIDPost']) && isset($_POST['characterNamePost'])&& isset($_POST['url_Picture']))
		{
			$user_IDPost = $_POST['userIDPost'];
			$characterNamePost = $_POST['characterNamePost'];
			$url_Picture = $_POST['url_Picture'];
			
			if($this->ConnectToDB())
			{
				if (!$this->CharacterExist($characterNamePost))
				{
					$req = self::$bdd->prepare('INSERT INTO t_KawaWorld_Character (User_ID,Character_Name,url_picture) VALUES(:user_ID, :character_Name, :url_Picture)');
					$req->execute(array('user_ID' => $user_IDPost,'character_Name' => $characterNamePost,'url_Picture' => $url_Picture));
					$req->closeCursor();
					
					$success = true;	
				}
				else
				{
					$jsonRequest->error= "Character name already exist";
				}	
			}	
			else
			{
				$jsonRequest->error= "Connection error";
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

		
	function RequestPlayers()
	{
		if($this->ConnectToDB())
		{
			$req = self::$bdd->prepare('SELECT u.* FROM t_kawaworld_user AS u');
			$req->execute();
			$results = $req->fetchall();

			$req->closeCursor();
		}
	}		

	function NewCharacter()
	{
		///TOO OLD
		$firstnamePost = ($_POST['firstnamePost']);
		$lastnamePost = ($_POST['lastnamePost']);
		$emailPost = strtolower($_POST['emailPost']);
		$Password = ($_POST['passwordPost']);
		
		if($this->ConnectToDB())
		{
			if($this->MailIsValid())
			{
				if ($this->MailExist())
				{
					self::$reporter->AddError("603");
				}
				else
				{
					$req = self::$bdd->prepare('INSERT INTO user(firstname,lastname,email,hiscore,country,facebook,password,newsletter) 												VALUES(:firstname, :lastname, :email, :hiscore, :country, :facebook, :password, :newsletter)');
					$req->execute(array('firstname' => self::$user->_FirstName,'lastname' => self::$user->_LastName,'email' => self::$user->_Email,'hiscore' => self::$user->_Hiscore,'country' => self::$user->_Country,'facebook' => self::$user->_Facebook,'password' => self::$user->_Password,'newsletter' => self::$user->_Newsletter,));
					$req->closeCursor();
				}
			}
		}
	}
		
//-----------------------VALIDE & EXIST--------------------------//

function MailExist($email)
{
	$req = self::$bdd->prepare('SELECT * FROM t_kawaworld_user WHERE Email = :email');
	$req->bindParam('email',$email);
	$req->execute();
	$count = $req->rowCount();
	$req->closeCursor();
	return($count > 0);
}

function LoginExist($login)
{
	$req = self::$bdd->prepare('SELECT * FROM t_kawaworld_user WHERE login = :login');
	$req->bindParam('login',$login);
	$req->execute();
	$count = $req->rowCount();
	$req->closeCursor();
	return($count > 0);
}
function CharacterExist($character)
{
	$req = self::$bdd->prepare('SELECT * FROM t_kawaworld_character WHERE Character_name = :character_name');
	$req->bindParam('character_name',$character);
	$req->execute();
	$count = $req->rowCount();
	$req->closeCursor();
	return($count > 0);
}

	function MailIsValid($email)
	{
		return (filter_var($email, FILTER_VALIDATE_EMAIL));
	}

    //----------------------Connect-------------------------//
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