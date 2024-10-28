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

- [Parts List README (PDF)](./pre-setup/R1_Parts_List_README.pdf)

---

## 2. 3D Printing Files

The robot's frame is 3D printed. All the necessary STL files and instructions for printing are included in the following folder. Please refer to the "Printing Guide PDF" for important details about printing the pieces:

- [STL Files for 3D Printing](./build-instructions/Printing_Files)
- [Printing Guide (PDF)](./build-instructions/R1_Printing_README.pdf)

---

## 3. Build Instructions

Once you have printed all the parts, it's time to assemble the robot. Follow the video link and refer to the provided assembly guide for detailed instructions:

- [Assembly Instructions PDF](./build-instructions/Instruction_README.pdf)
- [Build Instruction Video](https://youtu.be/2tXyRC_KmT0)

---

## 4. Software Setup

After building the robot, the next step is setting up the software. Most of the instructions are in video format, and the following index outlines the sequence to follow for setting everything up.

### Pre-Setup

Start by setting up the required API keys. You will need both an OpenAI API Key and an Azure Key.

- [Azure Key Setup Video](https://youtu.be/wxKWgDN9a2c)
- [OpenAI Key Setup Video](https://youtu.be/aMjvTZvdCsI)

---

### Robot Computer Setup

Next, set up the robot's computer to run the required software. Follow the instructions in the video below:

- [Computer Setup Video](https://youtu.be/bzLXkPkEXfA)

---

### Installing Unity

You will need to install a specific version of Unity to configure the robot. This guide covers installation for both Mac and Windows users:

- [Unity Installation Video (Cross-platform)](https://youtu.be/6DMLwrmqWGs)

---

### Starter Mode for Mac

Mac users should follow the instructions in the video below to set up Starter Mode for the robot:

- [Starter Mode Mac Video](https://youtu.be/YG3O_CxVHSQ)

---

### Starter Mode for Windows

Alternatively, Windows users can follow this video for setting up Starter Mode:

- [Starter Mode Windows Video](https://youtu.be/5EzZFH6TbZU)

---

### Face Adjustment

Once the software is set up, you may want to adjust the robot's projected face. This video provides a comprehensive overview of how to make these adjustments. Make sure to read the accompanying PDF for important projector notes:

- [Face Adjustment Video](https://youtu.be/Ks0d70vO0KM)
- [Projector README (PDF)](./face-adjustment/Projector_README.pdf)

---

### Pausing the Robot

To learn how to pause and resume the robot while it is running, refer to the following video:

- [Pausing the Robot Video](https://youtu.be/hnXKXJgQXd8)

---

### Custom Faces

You can add custom faces to the robot. Instructions are available for both Mac and Windows users. Additionally, the face template images referenced in the instructional videos are included:

- [Custom Faces for Mac Video](https://youtu.be/4W-XHF8ed94)
- [Custom Faces for Windows Video](https://youtu.be/J4PsrUzNvfo)
- [Face Template Images](./custom-faces/Face_Template_Images)

---

### Arduino Setup

The Arduino controls the head movement of the robot. Use the following instructions to set it up, and make sure to use the provided circuit diagram and Arduino sketches:

- [Arduino Setup Video](https://youtu.be/LZN1rNx5XJ8)
- [Circuit Diagram (PNG)](./arduino-setup/Circuit_Diagram.png)
- [Simple Sketch (Arduino Script)](./arduino-setup/Simple_Sketch.ino)
- [Advanced Sketch (Arduino Script)](./arduino-setup/Advanced_Sketch.ino)

---

### Advanced Mode

For more advanced functionality, such as modifying the robot's mouth sprites and other detailed features, follow the instructions in the advanced video below:

- [Advanced Mode Video](https://youtu.be/tfWZQt3Dw8M)

---

## 5. Unity Project Files

The R1 robot has two versions of the Unity project, each with different mouth sprites. The **r1-starter-mode-f** contains female mouth sprites, while **r1-starter-mode-m** contains male mouth sprites. You can choose which one to use based on your preference:

- [Starter Mode Female Project (Unity)](./r1-starter-mode-f)
- [Starter Mode Male Project (Unity)](./r1-starter-mode-m)

---

If you have any questions, feel free to reach out to us at [hello@stemlabkit.com](mailto:hello@stemlabkit.com). Enjoy building your R1 robot, and remember to follow all safety guidelines when working with tools and equipment!