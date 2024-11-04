# **Projet 7**

![image](https://github.com/user-attachments/assets/ce4ac0bd-baa5-4f46-8b4c-27661e561021)


## **Introduction**

![image](https://github.com/user-attachments/assets/71dc4118-7af8-4045-8faf-f476d3b064f0)


- Objectif de l'application
- Explication de son fonctionnement
- Démonstration
- Conclusion

## **Objectifs de l'application**

![Projet71](https://github.com/user-attachments/assets/29f1bf66-c11e-4891-a7c3-cb04f5889e1a)
![Projet75](https://github.com/user-attachments/assets/995e364f-a039-4694-ad33-3932ed47da6c)
![Projet74](https://github.com/user-attachments/assets/5a27178c-5824-46c8-a6cc-e1141c161501)
![Projet73](https://github.com/user-attachments/assets/10a74e57-a18f-4196-85a8-27290734b2db)
![Projet72](https://github.com/user-attachments/assets/ee525e7f-1e56-4f58-9bee-0963ccb57fcb)


Créer une application VR (Réalité Virtuelle) sur Unity, comportant une visite grâce des photos de la cité médiévale de Carcassonne et une autre visite grâce à des vidéos tout en VR.                  
Les visites doivent se faire grâce à des déplacements en téléportation et contenir un affichage des FPS.

## **Explication**

 - **XR Rig**:

 Le **XR Rig** dans Unity est un **Prefab** pré-configuré pour permettre le **déplacement**, les **mouvements** et les **interactions**, il inclut les **Controllers**, la **Caméra**, un **Character Controller** et les **Scripts** permettant le bon fonctionnement des principales mécanique, C'est grâce à lui que l'on interagit dans les scenes de l'application mais aussi qu'on se déplace sur un plane.

- **Teleportation Anchor**:

Ils sont des points de téléportation avec lesquels on peut **interagir** grâce aux **raycasts des controllers** pour les selectionner et se teleporter au prochain point, Ils permettent au personnage de se téléporter à travers la scene sélectionner et dans la scene en elle-même.

- **VideoPlayer**:

  le **VideoPlayer** un **composent** permettant de lire des vidéo mp4, il peut les lire à partir d'une **texture** ce qui crée un écran de lecture, les vidéos jouées dans la Vidéo Virtual Visit sont donc lu par le VidéoPlayer.

- **UI**:

  Il a été demandé d'afficher les FPS, ils sont affiché sur un **Canvas** en **WorldSpace** et des **TextMeshPro** contenu dans le canvas et un autre à été 
  
- **Skybox**:

  La Skybox est un Material Panoramique auquelle on affecte une image, elle permet d'afficher les images de la visite.
  
- **Bonus**:

  Pour rendre la visite plus ludique, un personnage en model 3D a été rajouté avec un animation pour les mains et une Visit avec des ennemis a été rajouté dans laquelle on retrouve la téléportion et l'interaction avec une épée pour mener le combat contre les ennemis.

## **Démonstration**

![Projet7](https://github.com/user-attachments/assets/61b7d4d4-3d6d-4288-9651-299b549f5fec)

## **Conclusion**

Pour Conclure, l'application est fonctionnelle, permet de choisir trois visite différentes, de ce déplacer en se téléportant et de faire des combats pour avancer ou de profité du paysage.

![true-romance-tony-scott](https://github.com/user-attachments/assets/6d7612a3-c492-4003-ad20-21d691f44156)

Lien APK : https://drive.google.com/file/d/1YeUv2HZGkrpvOshfrC3VCzq9linHFLQK/view?usp=drive_link
