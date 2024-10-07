# Portage du TP Sécurité Django sur un mac arm avec UTM/Qemu


## 1.	Installation des prérequis

L'installation des prérequis se fait avec homebrew.

```bash
brew install qemu
brew install utm
```

## 2.	Extraction de l'image disque et conversion en format qcow2

tar -xvf pton2-tp1-1_0_0.ovf
qemu-img convert -f vmdk -O qcow2 pton2-tp1-1_0_0-disk001.vmdk pton2-tp1-1_0_0-disk001.qcow2

L'image fournie au format ovf n'est pas directement utilisable par UTM. Il convient donc d'extraire l'archive, et de convertir son contenu en qcow2, un format de disque virtuel utilisable par UTM.

```bash
tar -xvf pton2-tp1-1_0_0.ovf
qemu-img convert -f vmdk -O qcow2 pton2-tp1-1_0_0-disk001.vmdk pton2-tp1-1_0_0-disk001.qcow2
```

## 3.	Création de la machine virtuelle

Ouvrir UTM et cliquer sur "File", puis "New", puis "Emulate". Ensuite, choisir "None" pour le template, puis "None" pour boot device (pas besoin de disque d'installation puisque le disque virtuel contient déjà l'OS installé). Enfin, cliquer sur "Continue" jusqu'à la fin, en laissant les valeurs par défaut, puis "Create".

**Note:** Vous pouvez changer le nom de la machine virtuelle à la dernière étape, avant de cliquer sur "Create".

## 4. Ajout du disque virtuel et configuration réseau

Sélectionner la machine virtuelle, puis cliquer sur le bouton de configuration (curseurs en haut à droite).

![image](https://mac.getutm.app/images/configuration.png)

Dans le panneau de droite, cliquer sur le disque virtuel `IDE Drive`, et supprimez le. Ensuite, créez un nouveau disque virtuel, et importez le fichier pton2-tp1-1_0_0-disk001.qcow2.

Enfin, dans l'onglet "Network", sélectionner le mode "Emulated VLAN", et allez dans le sous-onglet "Port Forwarding". Créer les deux règles suivantes:

- Protocol: TCP, Guest Address: None, Guest Port: 5000, Host Address: 127.0.0.1, Host Port: 4444
- Protocol: TCP, Guest Address: None, Guest Port: 8000, Host Address: 127.0.0.1, Host Port: 4445
