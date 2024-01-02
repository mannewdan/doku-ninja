using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Notifications {
  public const string DEBUG = "DEBUG_NOTIFICATION";
  public const string DEBUG_CTRL = "DEBUG_CTRL_NOTIFICATON";
  public const string RESIZE = "RESIZE_NOTIFICATION";
  public const string MOVE = "MOVE_NOTIFICATION";
  public const string MOVE_REPEAT = "MOVE_MULTI_NOTIFICATION";
  public const string NUMBER = "NUMBER_NOTIFICATION";
  public const string CONTROL_NUMBER = "CONTROL_NUMBER_NOTIFICATION";
  public const string TAB = "TAB_NOTIFICATION";
  public const string CONFIRM = "CONFIRM_NOTIFICATION";
  public const string CANCEL = "CANCEL_NOTIFICATION";
  public const string START = "START_NOTIFICATION";
  public const string DISCARD = "DISCARD_NOTIFICATION";
  public const string SAVE = "SAVE_NOTIFICATION";
  public const string RESET = "RESET_NOTIFICATION";
  public const string SHIFT_HELD = "SHIFT_HELD_NOTIFICATION";
  public const string SHIFT_RELEASED = "SHIFT_RELEASED_NOTIFICATION";

  public const string MAP_SOLVED = "MAP_SOLVED_NOTIFICATION";

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

  public const string BOMB_STATUS_CHANGED = "BOMB_STATUS_CHANGED_NOTIFICATION";
  public const string BOMB_COUNTDOWN = "BOMB_COUNTDOWN_NOTIFICATON";
  public const string BOMB_PRIMED = "BOMB_PRIMED_NOTIFICATION";
  public const string BOMB_EXPLODED = "BOMB_EXPLODED_NOTIFICATION";
  public const string BOMB_REMOVED = "BOMB_DESTROYED_NOTIFICATION";
  public const string BOMB_ADD_TARGET = "BOMB_ADD_TARGET_NOTIFICATION";
  public const string BOMB_REMOVE_TARGET = "BOMB_REMOVE_TARGET_NOTIFICATION";

  public const string MAP_WALL_CHANGED = "MAP_WALL_CHANGED_NOTIFICATION";
  public const string MAP_SIZE_CHANGED = "MAP_SIZE_CHANGED_NOTIFICATION";
}
