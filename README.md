# R1 DIY Robot Kit

![Assembled R1 DIY Robot](./images/main_robot_photo.png)
*Assembled R1 DIY Robot with face projection, ready for interaction.*

Welcome to the **R1 DIY Kit**! These files contain every step needed to manufacture and build your very own R1 robot. This DIY kit is based on the original R1 design, with the same internal design and hardware. 

While most instructions are provided in video format, important details can also be found in the accompanying README PDFs throughout the repository. Please read the provided documentation carefully to ensure successful assembly and setup.

## Table of Contents

1. [Gather Required Parts](#gather-required-parts)
2. [3D Printing Files](#3d-printing-files)
3. [Build Instructions](#build-instructions)
4. [Software Setup](#software-setup)
   - [Pre-Setup](#pre-setup)
   - [Robot Computer Setup](#robot-computer-setup)
   - [Installing Unity](#installing-unity)
   - [Starter Mode for Mac](#starter-mode-for-mac)
   - [Starter Mode for Windows](#starter-mode-for-windows)
   - [Face Adjustment](#face-adjustment)
   - [Pausing the Robot](#pausing-the-robot)
   - [Custom Faces](#custom-faces)
   - [Arduino Setup](#arduino-setup)
   - [Advanced Mode](#advanced-mode)
5. [Unity Project Files](#unity-project-files)

---

## 1. Gather Required Parts

Before you begin building, print the files and gather all the necessary parts. Here is the bill of materials with links to the components you will need to purchase:

- [Parts List README (PDF)](./PRE_SETUP/R1_Parts_List_README.pdf)

---

## 2. 3D Printing Files

The robot's frame is 3D printed. All the necessary STL files and instructions for printing are included in the following folder. Please refer to the "Printing Guide PDF" for important details about printing the pieces:

- [STL Files for 3D Printing](./BUILD_INSTRUCTIONS/Printing_Files)
- [Printing Guide (PDF)](./BUILD_INSTRUCTIONS/R1_Printing_README.pdf)

---

## 3. Build Instructions

Once you have printed all the parts, it's time to assemble the robot. Follow the video link and refer to the provided assembly guide for detailed instructions:

- [Assembly Instructions PDF](./BUILD_INSTRUCTIONS/Instruction_README.pdf)
- [Build Instruction Video (YouTube Link)](./BUILD_INSTRUCTIONS/Build_Instructions_Video_Link.md)

---

## 4. Software Setup

After building the robot, the next step is setting up the software. Most of the instructions are in video format, and the following index outlines the sequence to follow for setting everything up.

### Pre-Setup

Start by setting up the required API keys. You will need both an OpenAI API Key and an Azure Key.

- [Azure Key Setup Video](./PRE_SETUP/AzureKey_Video_Link.md)
- [OpenAI Key Setup Video](./PRE_SETUP/OpenAIKey_Video_Link.md)

---

### Robot Computer Setup

Next, set up the robot's computer to run the required software. Follow the instructions in the video below:

- [Computer Setup Video](./ROBO_COMP_SETUP/ComputerSetup_Video_Link.md)

---

### Installing Unity

You will need to install a specific version of Unity to configure the robot. This guide covers installation for both Mac and Windows users:

- [Unity Installation Video (Cross-platform)](./INSTALLING_UNITY/UnityInstallCrossPlatform_Video_Link.md)

---

### Starter Mode for Mac

Mac users should follow the instructions in the video below to set up Starter Mode for the robot:

- [Starter Mode Mac Video](./STARTERMODEMAC/StarterModeMac_Video_Link.md)

---

### Starter Mode for Windows

Alternatively, Windows users can follow this video for setting up Starter Mode:

- [Starter Mode Windows Video](./STARTERMODEWIN/StarterModeWin_Video_Link.md)

---

### Face Adjustment

Once the software is set up, you may want to adjust the robot's projected face. This video provides a comprehensive overview of how to make these adjustments. Make sure to read the accompanying PDF for important projector notes:

- [Face Adjustment Video](./FACE_ADJUSTMENT/FaceAdjustment_Video_Link.md)
- [Projector README (PDF)](./FACE_ADJUSTMENT/Projector_README.pdf)

---

### Pausing the Robot

To learn how to pause and resume the robot while it is running, refer to the following video:

- [Pausing the Robot Video](./PAUSING_ROBOT/PausingRobot_Video_Link.md)

---

### Custom Faces

You can add custom faces to the robot. Instructions are available for both Mac and Windows users. Additionally, the face template images referenced in the instructional videos are included:

- [Custom Faces for Mac](./CUSTOM_FACES/Custom_Face_Mac.md)
- [Custom Faces for Windows](./CUSTOM_FACES/Custom_Face_Win.md)
- [Face Template Images](./CUSTOM_FACES/Face_Template_Images)

---

### Arduino Setup

The Arduino controls the head movement of the robot. Use the following instructions to set it up, and make sure to use the provided circuit diagram and Arduino sketches:

- [Arduino Setup Instructions](./ARDUINO_SETUP/Arduino_Instructions.md)
- [Circuit Diagram (PNG)](./ARDUINO_SETUP/Circuit_Diagram.png)
- [Simple Sketch (Arduino Script)](./ARDUINO_SETUP/Simple_Sketch.ino)
- [Advanced Sketch (Arduino Script)](./ARDUINO_SETUP/Advanced_Sketch.ino)

---

### Advanced Mode

For more advanced functionality, such as modifying the robot’s mouth sprites and other detailed features, follow the instructions in the advanced video below:

- [Advanced Mode Video](./ADVANCED_MODE/Advanced_Mode_Video_Link.md)

---

## 5. Unity Project Files

The R1 robot has two versions of the Unity project, each with different mouth sprites. The **Starter_Mode_F** contains female mouth sprites, while **Starter_Mode_M** contains male mouth sprites. You can choose which one to use based on your preference:

- [Starter Mode Female Project (Unity)](./R1_STARTER_MODE_F)
- [Starter Mode Male Project (Unity)](./R1_STARTER_MODE_M)

---

If you have any questions, feel free to reach out to us at [hello@stemlabkit.com](mailto:hello@stemlabkit.com). Enjoy building your R1 robot, and remember to follow all safety guidelines when working with tools and equipment!
