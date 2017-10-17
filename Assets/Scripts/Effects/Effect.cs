using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect  {
    bool update(bool isPlayerTurn,Entity affected);
    void apply(Entity affected);
    string ToString();  
}
