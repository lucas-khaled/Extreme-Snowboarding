using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void StateStart(Player player);

    void StateEnd();

    void StateUpdate();

    void InterpretateInput(GameInput input);
}
