# FDV Prototipo juego 2D

This Project combines all the techniques worked throughout the subject

## Introduction

In this project we'll controll an adventurer avoiding and defeating enemies and interacting with other elements in the level. This project is the culmination of everything we work on in this discipline.

## The level

This game has two Scene: one being the menu and the other being the first level. We’ll have moving platforms, enemies, and collectibles.  The Player must move through the level to get to the final door to win the game.

   ![The Level](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/thelevel.png)

### TileMaps and Layers

The level was built using a [rectangular Tilemap](<https://assetstore.unity.com/packages/2d/environments/nature-pixel-art-base-assets-free-151370>)  which was added an Tilemap collider 2D, Composite Collider, to make sure the total and not the individual colliders are considered to manage the collision and an RigidBody 2D set to static. The tilemaps are divided into 3 layers, one for the floor of the level, another for the walls surrounding the level and finally one for the platforms and obstacles in the level, each of them assigned a tag and a layer, so the collisions could be managed more easily.
We have 11 layers interacting with each other as shown in the image below.

   ![TileMaps and Layers](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/layes_col.png)

### Parallax

The Level uses a Parallax Effect achieved by using 2 texturized Quads, they are children of the main camara following the player movement and readjusting the texture taking the player’s movement into account. The background will be a combination of a sky, a mountain and trees.
To achieve this effect we used 2 scripts: Parallax and ParallaxCam, being the Parallax script being attached to an empty game object named GameManager which manages the speed and references the camara target by the ParallaxCam script which tells the its movement so the position of the quads could be readjusted.

   ![Parallax](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/Parallax.gif)

### Moving Platforms

Through the level the player will interact with some moving platforms, some must be activated first so it can be moved. The platforms work by moving from one objective which is an empty game object to another objective.

   ![Moving Platforms](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/MovingPlatforms.gif)

## Characters and Animations

In this project we have 3 characters, an adventurer, a ghost, and zombie. Each of these characters have their own animations and their own transitions.

### Player

The player is composed of a sprite with attributes set to multiple so it could be used the multiple elements, to that sprite there’s attached an RigidBody 2D, a box collider, PlayerMov (responsible managing the player’s movements, the animations, etc) and Weapon (for managing the shoot event) scripts. To his RigidBody 2D is added a Physic Material with zero friction and bounciness to avoid the getting stuck in the tilemaps.

   ![Player](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/Player.png)

#### PlayerMov Script

This Scripts manages the player movements by taking the values of the horizontal Axis and use it to move/jump the player character on the conditions that the player is touching the Floor. We check if the player is touching the ground if his collider is colling with the tilemap that has the floor tag If the value of the horizontal Axis is less than zero, we flip the character sprite and rotate the turret game object that we used to shoot the projectile.
We also manage the players Health by taking the damage value that the enemy has and subtracting from the player’s health and we apply an vibration to the camera and if it touches a coin we add one point to his score.

```cs
        horizontalMove = Input.GetAxisRaw("Horizontal") * mov_Speed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            m_Rigidbody2D.AddForce(m_JumpForce * Vector2.up);
        }

        if (animator)
        {
            animator.SetBool("IsJumping", !isGrounded);
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        if (CharacterSprite)
        {
            if (horizontalMove > 0)
            {
                CharacterSprite.flipX = false;
            }
            else if (horizontalMove < 0)
            {
                CharacterSprite.flipX = true;
            }
        }

        public void MovePlayer(float move)
        {
         Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
         m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }

        public void HealthManaager(int value)
        {
            PlayerHealth += value;
            if (PlayerHealth < 100)
        {
            PlayerHealth = 200;
        }
        }
        
        public void ShakeCam(float intensity, float time)
        {
            CinemachineBasicMultiChannelPerlin camaraNoise = Camara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (Camara != null)
            {
                camaraNoise.m_AmplitudeGain = intensity;
                camaraNoise.m_FrequencyGain = intensity;
                shakerTimer = time;
            }
        }
```

#### Player's Animations

The player has 5 animations, one for when he’s idle, one for walking, one for jumping and one when his health reaches zero.
We also have parameters to allow the transitions between them:

- Speed is being updated by the absolute value of the horizontal Axis.
- IsJumping for when the player is not touching the floor.
- IsDead when his health reaches 0.

    ![Player's Animations](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/PlayerAnimator.png)

When the speed is equal a zero the idle animation is played and when its different the walk animations is played. When the Boolean isJumping is set to true the jump animations is played and when his health reaches zero the IsDead Boolean is set to true and the animation IsHurt is played.

### Ghost

The ghost gameobject is composed of a sprite attached with the ghost script, an RigidBody 2D, and box collider set to IsTrigger. The ghost character can go through walls, float in the space and will attach the player if near him. And every 100 seconds we will spawn one ghost to attach the player and this prefab is also being pooled.

   ![Ghost](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/ghost.png)

#### Ghost Script

This Scripts manages as with the player’s script manages the character’s sprite flipping animations and his movements.  The Ghost can only move if he can see the player and that is checked by drawing a Linecast from the ghost’s position to the player’s and if the distance is less 5 units taking into the account the layer, we set the variable can see player to true and that allow the ghost to move to the players position.
XXXXXXXXXX
We are checking its collisions with other elements by checking the tag of the collided element if it’s the player we are inflicting damage and subtracting the player’s health and if it’s the bullet we are dealing damage and subtracting the ghost’s health.

```cs
       _reWalk = Physics2D.Linecast(transform.position, playerTrans.position, detetorLayermask);

        if (_reWalk.collider != null)
        {
            if (_reWalk.collider.name == "Player" && _reWalk.distance < 5.0f)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }

        if (animator)
        {
            animator.SetFloat("Distance", Mathf.Abs(_reWalk.distance));
        }

        if (CharacterSprite && canSeePlayer)
        {
            if (DistancePlayer() > 0)
            {
                CharacterSprite.flipX = true;
            }
            else if (DistancePlayer() < 0)
            {
                CharacterSprite.flipX = false;
            }
        }
        if (GhostHealth == 0 || GhostHealth < 0)
        {
            Destroy(gameObject);
        }

         private void FixedUpdate()
        {
            if (canSeePlayer)
            {
                m_Rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, playerTrans.position, mov_Speed * Time.deltaTime));
            }
        }

         private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerMov>().HealthManaager(damage);
                PlayerMov.Instance.ShakeCam(.7f, 0.1f);
            }
            if (other.gameObject.CompareTag("Bala"))
            {
                GhostHealth -= 20;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerMov>().HealthManaager(damage / 10);
                PlayerMov.Instance.ShakeCam(.5f, 0.1f);
            }
        }

```

### Ghost Animations

The Ghost has 3 animations, one for when he’s idle, one for walking and one for his attacks. We have one parameter (Distance) to manage to manage the transitions between them:

- When its bellow 1 units the attack animation is played.
- The walk animation is played when the distance is between 6 and 9 units, greater than 10 the idle animation is played.

    ![Ghost Animations](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/GhostAnimator.png)

### Zombie

The Zombie gameobject is composed of a sprite attached with the Zombie script, an RigidBody 2D, and box collider. The zombie character is spawned when the player triggers a collider, and it must be defeated so the static platforms be activated.

   ![Zombie](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/Zombie.png)

### Zombie Script

This Scripts manages like Ghost’s Script manages the character’s sprite flipping animations and his movements.  The zombie also can only move if he can see the player and that is checked by drawing a Linecast from its position to the player’s and if the distance is less 6 units taking into the account the layer, we set the variable can see player to true and that allow the ghost to move to the players position. It also contains a method to manage the zombie’s health.

```cs
        if (_reWalk.collider != null)
        {
            if (_reWalk.collider.name == "Player" && _reWalk.distance < 6.0f)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }

        if (animator)
        {
            animator.SetBool("IsDead", true);
            animator.SetFloat("Distance", Mathf.Abs(_reWalk.distance));
        }
        public void ManageZombieHealth(int damage)
        {
            ZombieHealth -= damage;
        }
```

### Zombie's Animations

The Zombie has 4 animations, one for when he’s idle, one for walking, one for his attacks and one for when he’s defeated. We have 3 parameters to manage to manage the transitions between them:

- When the Distance between the player and its position is less than 2 units the attack animation is played and when its between 3 and 10 units the the walk animation and ehrn its greater than 10 the idle animation is played. The IsDead when set to true plays the death animation.

   ![Zombie's Animations](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/ZombieAnimator.png)

## Camera

To the addiction of the main camera, we added 2 more cinemachine’s camara: one 2D camara and the other being TargetGroup Camara. The 2D camara follows the player throughout the level and the target camara is activated when the player triggers an event, targeting the player and the zombie and it’s deactivated after the zombie’s defeat. We also added a confiner to the restrict the camara movements and in the player’s attached script there’s a function that alters the frequency and the gain of the camara’s noise.

   ![Camera](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/camara.png)

   ![Camera Shake](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/Camerashake.gif)

## Other Elements

### Coins

Through the level there will be Coins objects that the player can collect increasing his score.

### Bullet

The bullet is a prefab that is instantiated when the player shoots. It checks the tags of the elements it collides with and based on the tags executes a method and self-destructs. This prefab is used by the ObjectPooler script to optimize the performance and the load of its multiple’s executions.
XXXXXXXXXXXXX

### Events

There are 2 events, one for when the players enter the zombie dungeons and the other for when the players die.
When the players trigger the event the GameManager objects executes the method that was subscribed to the event and deactivates the 2D camara and activates the targer group camara, sets to active the zombie and to false the door and the triggers which are then reversed when the zombie is defeated, and its attached event is executed.

```cs
    public delegate void OnRecoil();
    public event OnRecoil ZombieDeath;

    if (ZombieHealth == 0 || ZombieHealth < 0)
        {
            ZombieDeath();
            Destroy(gameObject);
        }
        
    public delegate void OnRecoil();
    public event OnRecoil StartZombie;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("player entra");
            StartZombie();
        }
    }
```

### Menu

The menu has the options to play, pause and quit the game. It has an SceneManager component which loads the different scenes.

   ![Menu](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/Menu.png)

### Teleportation

To enter the zombie’s dungeon the player collider, must touch the door atop of the dungeon and it will be teleported to the door inside the dungeon, this is done by getting the reference to the door’s position and set it to the player’s position. When this happens the event StartZombie event is executed.

Demo:

   ![Demo](https://github.com/almadacv/FDV-Prototipo-juego-2D/blob/main/Gif/demo.gif)
