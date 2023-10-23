using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Notifications {
  public const string DEBUG = "DEBUG_NOTIFICATION";
  public const string RESIZE = "RESIZE_NOTIFICATION";
  public const string MOVE = "MOVE_NOTIFICATION";
  public const string MOVE_REPEAT = "MOVE_MULTI_NOTIFICATION";
  public const string NUMBER = "NUMBER_NOTIFICATION";
  public const string TAB = "TAB_NOTIFICATION";
  public const string CONFIRM = "CONFIRM_NOTIFICATION";
  public const string CANCEL = "CANCEL_NOTIFICATION";
  public const string DISCARD = "DISCARD_NOTIFICATION";

  public const string PLAYER_PHASE_START = "PLAYER_PHASE_START_NOTIFICATION";
  public const string PLAYER_PHASE_END = "PLAYER_PHASE_END_NOTIFICATION";
  public const string PLAYER_SPENT_AP = "PLAYER_SPENT_AP_NOTIFICATION";
  public const string PLAYER_GAINED_AP = "PLAYER_GAINED_AP_NOTIFICATION";
  public const string PLAYER_CHANGED_AP_MAX = "PLAYER_CHANGED_AP_MAX_NOTIFICATION";
  public const string PLAYER_HARMED = "PLAYER_HARMED_NOTIFICATION";
  public const string PLAYER_DEAD = "PLAYER_DEAD_NOTIFICATION";

  public const string ENEMY_PHASE_START = "ENEMY_PHASE_START_NOTIFICATION";
  public const string ENEMY_PHASE_END = "ENEMY_PHASE_END_NOTIFICATION";
  public const string ENEMY_ROUND_START = "ENEMY_ROUND_START_NOTIFICATION";
  public const string ENEMY_ROUND_END = "ENEMY_ROUND_END_NOTIFICATION";

  public const string UNIT_MOVED = "UNIT_MOVED_NOTIFICATION";
  public const string UNIT_DESTROYED = "UNIT_DESTROYED_NOTIFICATION";
  public const string UNIT_ADD_TARGET = "UNIT_ADD_TARGET_NOTIFICATION";
  public const string UNIT_REMOVE_TARGET = "UNIT_REMOVE_TARGET_NOTIFICATION";
  public const string UNIT_ACTIVE_CHANGED = "UNIT_ACTIVE_CHANGED_NOTIFICATION";
  public const string UNIT_DAMAGED = "UNIT_DAMAGED_NOTIFICATION";

  public const string CARD_DRAW = "CARD_DRAW_NOTIFICATION";
  public const string CARD_DISCARD = "CARD_DISCARD_NOTIFICATION";
  public const string CARD_ACTIVE = "CARD_ACTIVE_NOTIFICATION";
  public const string CARD_INACTIVE = "CARD_INACTIVE_NOTIFICATION";

  public const string TILE_WALL_CHANGED = "WALL_CHANGED_NOTIFICATION";
}
