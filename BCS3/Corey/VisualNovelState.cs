using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BCS_3
{
    internal class VisualNovelState : GameState
    {
        Color bcsBlue = new Color(174, 225, 243);
        Color bcsOrange = new Color(246, 138, 39);
        Color bcsWhite = Color.White;

        string scriptName; 

        bool firstTime = true;
        bool char1Disp = true;
        bool char2Disp = true;
        bool char1Wide = false;
        bool char2Wide = false; 
        bool visualNovelComplete = false; 

        int screenHeight;
        int screenWidth;
        int dialogueCount = 8;

        string currentSpeaker = "";
        string currentDialogue1 = "";
        string currentDialogue2 = ""; 

        string[] visualNovelLines;

        KeyboardState oldKeyboardState; 

        Rectangle dialogueBox;
        Rectangle char1Pos;
        Rectangle char1PosWide; 
        Rectangle char2Pos;
        Rectangle char2PosWide; 
        Rectangle bgPos; 

        Vector2 boxNamePos;
        Vector2 boxLine1Pos;
        Vector2 boxLine2Pos;

        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        Dictionary<string, Song> songs = new Dictionary<string, Song>(); 

        Texture2D currentBackground; 
        Texture2D currentCharacter1;
        Texture2D currentCharacter2;

        SpriteFont nameFont;
        SpriteFont speakFont; 

        public VisualNovelState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager, string scriptName) : base(graphicsDevice, spriteBatch, contentManager)
        {
            this.screenHeight = graphicsDevice.Viewport.Height;
            this.screenWidth = graphicsDevice.Viewport.Width;

            this.scriptName = scriptName;

            this.visualNovelLines = File.ReadAllLines("Corey/scripts/" + this.scriptName);

            this.dialogueBox = new Rectangle(0, (int)(this.screenHeight * (75f / 100f)), this.screenWidth, this.screenHeight / 4);
            this.char1Pos = new Rectangle((int)(this.screenWidth * .05), (int)(this.screenHeight * .12), (int)(this.screenWidth * .2), (int)(this.screenHeight * .65));
            this.char1PosWide = new Rectangle((int)(this.screenWidth * .05), (int)(this.screenHeight * .12), (int)(this.screenWidth * .35), (int)(this.screenHeight * .65));
            this.char2Pos = new Rectangle((int)(this.screenWidth * .75), (int)(this.screenHeight * .12), (int)(this.screenWidth * .2), (int)(this.screenHeight * .65));
            this.char2PosWide = new Rectangle((int)(this.screenWidth * .6), (int)(this.screenHeight * .12), (int)(this.screenWidth * .35), (int)(this.screenHeight * .65));
            this.bgPos = new Rectangle(0, 0, this.screenWidth, this.screenHeight); 

            this.boxNamePos = new Vector2((int)(this.screenWidth * .03), (int)(this.screenHeight * .75));
            this.boxLine1Pos = new Vector2((int)(this.screenWidth * .06), (int)(this.screenHeight * .85));
            this.boxLine2Pos = new Vector2((int)(this.screenWidth * .06), (int)(this.screenHeight * .92));

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Draw(this.currentBackground, this.bgPos, this.bcsWhite);
            if (this.char1Disp)
            {
                if(!this.char1Wide)
                {
                    SpriteBatch.Draw(this.currentCharacter1, this.char1Pos, this.bcsWhite);
                }
                else
                {
                    SpriteBatch.Draw(this.currentCharacter1, this.char1PosWide, this.bcsWhite);
                }
            }

            if (this.char2Disp)
            {
                if(!this.char2Wide)
                {
                    SpriteBatch.Draw(this.currentCharacter2, this.char2Pos, this.bcsWhite);
                }
                else
                {
                    SpriteBatch.Draw(this.currentCharacter2, this.char2PosWide, this.bcsWhite);
                }
            }
            SpriteBatch.Draw(this.textures["rectangleBase"], this.dialogueBox, this.bcsOrange);
            SpriteBatch.DrawString(this.nameFont, this.currentSpeaker, this.boxNamePos, this.bcsWhite);
            SpriteBatch.DrawString(this.speakFont, this.currentDialogue1, this.boxLine1Pos, this.bcsWhite);
            SpriteBatch.DrawString(this.speakFont, this.currentDialogue2, this.boxLine2Pos, this.bcsWhite);


            SpriteBatch.End(); 
            
        }

        public override void Update(GameTime gameTime)
        {

            if(this.firstTime)
            {
                string songName = this.visualNovelLines[1].Trim(); 
                string firstBackground = this.visualNovelLines[3].Trim(); 
                string[] firstChracters = this.visualNovelLines[5].Trim().Split('&');
                string[] firstDialogue = this.visualNovelLines[7].Trim().Split('&');

                MediaPlayer.Play(this.songs[songName]);
                MediaPlayer.IsRepeating = true; 

                this.currentBackground = textures[firstBackground];

                string char1 = firstChracters[0].Trim();
                string char2 = firstChracters[1].Trim();

                if (char1 == "")
                {
                    this.char1Disp = false; 
                    this.currentCharacter1 = this.textures["nothing"];
                }
                else
                {
                    this.char1Disp = true;
                    this.currentCharacter1 = this.textures[char1];

                    if (char1 == "steelBeamBoy" || char1 == "samsung" || char1 == "cancerInstallationMan") this.char1Wide = true;
                    else this.char1Wide = false; 
                }

                if (char2 == "")
                {
                    this.char2Disp = false; 
                    this.currentCharacter2 = this.textures["nothing"];
                }
                else
                {
                    this.char2Disp = true; 
                    this.currentCharacter2 = this.textures[char2];

                    if (char2 == "steelBeamBoy" || char2 == "samsung" || char2 == "cancerInstallationMan") this.char2Wide = true;
                    else this.char2Wide = false; 
                }

                this.currentSpeaker = firstDialogue[0].Trim();
                this.currentDialogue1 = firstDialogue[1].Trim();
                this.currentDialogue2 = firstDialogue[2].Trim(); 

                this.firstTime = false; 
            }

            if (this.visualNovelComplete) return; 

            KeyboardState keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
            {
                if (this.visualNovelLines[this.dialogueCount].Trim() == "SONG")
                {
                    this.dialogueCount++;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(this.songs[this.visualNovelLines[this.dialogueCount].Trim()]);
                    MediaPlayer.IsRepeating = true; 
                    this.dialogueCount++;
                }

                if (this.visualNovelLines[this.dialogueCount].Trim() == "BACKGROUND")
                {
                    this.dialogueCount++;
                    this.currentBackground = this.textures[this.visualNovelLines[this.dialogueCount].Trim()];
                    this.dialogueCount++; 
                }

                if(this.visualNovelLines[this.dialogueCount].Trim() == "CHARACTERS")
                {
                    this.dialogueCount++;
                    string[] currentLine = visualNovelLines[this.dialogueCount].Split('&');

                    string char1 = currentLine[0].Trim();
                    string char2 = currentLine[1].Trim();

                    Console.WriteLine(char1); 
                    Console.WriteLine(char2);

                    if (char1 == "")
                    {
                        this.char1Disp = false;
                        this.currentCharacter1 = this.textures["nothing"];
                    }
                    else
                    {
                        this.char1Disp = true;
                        this.currentCharacter1 = this.textures[char1];

                        if (char1 == "steelBeamBoy" || char1 == "samsung" || char1 == "cancerInstallationMan") this.char1Wide = true;
                        else this.char1Wide = false;
                    }

                    if (char2 == "")
                    {
                        this.char2Disp = false;
                        this.currentCharacter2 = this.textures["nothing"];
                    }
                    else
                    {
                        this.char2Disp = true;
                        this.currentCharacter2 = this.textures[char2];

                        if (char2 == "steelBeamBoy" || char2 == "samsung" || char2 == "cancerInstallationMan") this.char2Wide = true;
                        else this.char2Wide = false;
                    }

                    this.dialogueCount++; 
                    
                }

                if(this.visualNovelLines[this.dialogueCount].Trim() == "DIALOGUE")
                {
                    this.dialogueCount++;
                    string[] currentLine = visualNovelLines[this.dialogueCount].Split('&');
                    this.currentSpeaker = currentLine[0].Trim();
                    this.currentDialogue1 = currentLine[1].Trim();
                    this.currentDialogue2 = currentLine[2].Trim();

                    this.dialogueCount++; 
                }

                this.dialogueCount++;

                if (this.dialogueCount == this.visualNovelLines.Length) this.visualNovelComplete = true; 
            }

            if (this.dialogueCount+1 >= this.visualNovelLines.Length) this.visualNovelComplete = true;


            this.oldKeyboardState = keyboardState; 
        }

        protected override void LoadContent(ContentManager contentManager)
        {
            this.textures["nothing"] = contentManager.Load<Texture2D>("corey/backgrounds/rectangleBase");
            this.textures["ruinedWorld"] = contentManager.Load<Texture2D>("corey/backgrounds/ruinedWorld");
            this.textures["dayVillage"] = contentManager.Load<Texture2D>("corey/backgrounds/dayVillage");
            this.textures["farm"] = contentManager.Load<Texture2D>("corey/backgrounds/farm");
            this.textures["ogBcs"] = contentManager.Load<Texture2D>("corey/backgrounds/home");
            this.textures["nightVillage"] = contentManager.Load<Texture2D>("corey/backgrounds/nightVillage");
            this.textures["store"] = contentManager.Load<Texture2D>("corey/backgrounds/store");
            this.textures["rectangleBase"] = contentManager.Load<Texture2D>("corey/backgrounds/white");
            this.textures["space"] = contentManager.Load<Texture2D>("corey/backgrounds/space");

            this.textures["bcsMan"] = contentManager.Load<Texture2D>("corey/characters/bcman");
            this.textures["bcsManBloody"] = contentManager.Load<Texture2D>("corey/characters/bcman_done");
            this.textures["broccoli"] = contentManager.Load<Texture2D>("corey/characters/broccoli");
            this.textures["broccoliChildren"] = contentManager.Load<Texture2D>("corey/characters/broccoli_children");
            this.textures["noveltryMan"] = contentManager.Load<Texture2D>("corey/characters/noveltryMan");
            this.textures["protag"] = contentManager.Load<Texture2D>("corey/characters/protag");
            this.textures["protagBocarina"] = contentManager.Load<Texture2D>("corey/characters/protagBocarina");
            this.textures["tree"] = contentManager.Load<Texture2D>("corey/characters/tree");
            this.textures["villager"] = contentManager.Load<Texture2D>("corey/characters/villager");
            this.textures["steelBeamBoy"] = contentManager.Load<Texture2D>("corey/characters/steelBeamBoy");
            this.textures["samsung"] = contentManager.Load<Texture2D>("corey/characters/samsung");
            this.textures["bcsBoy"] = contentManager.Load<Texture2D>("corey/characters/bcsBoy");
            this.textures["cancerInstallationMan"] = contentManager.Load<Texture2D>("corey/characters/cancerInstallationMan");

            this.songs["darkAndGritty"] = contentManager.Load<Song>("corey/music/fight"); 

            this.speakFont = contentManager.Load<SpriteFont>("corey/fonts/bcsFont");
            this.nameFont = contentManager.Load<SpriteFont>("corey/fonts/bcsFont2");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
