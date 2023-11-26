using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UsersModel
{
    public UsersModel(string name,int puntaje,string base64Image) {
        this.Name = name;
        this.Puntaje = puntaje;
        this.Base64Image = base64Image;
    }


    public string Name { get; set; }

    public int Puntaje { get; set; }

    public string Base64Image { get; set;}
    
}
