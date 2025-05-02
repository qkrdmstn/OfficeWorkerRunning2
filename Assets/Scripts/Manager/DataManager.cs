using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    static public DataManager instance;

    [Header("Recording info")]
    public int recordingFrame;
    public int delayFrame = 45;
    public SortedList<int, Command> recordedCommands = new SortedList<int, Command>();
    public List<SerializableCommand> serializableCommands = new List<SerializableCommand>();

    [Header("SnapShot info")]
    public Queue<ControllerSnapshot> snapshots = new Queue<ControllerSnapshot>();
    [SerializeField] private int snapshotInterval = 1;


    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.IsPlaying() || SceneManager.GetActiveScene().name != "Stage")
            return;

        if (!GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
        {
            recordingFrame++;
            SaveSnapshot();
        }
    }

    private void SaveSnapshot()
    {
        if (recordingFrame % snapshotInterval == 0)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            snapshots.Enqueue(new ControllerSnapshot(recordingFrame, player));

            if (snapshots.Count > delayFrame)
                snapshots.Dequeue();
        }
    }

    public void SaveExcuteCommand(Command command)
    {
        recordedCommands.Add(recordingFrame, command);
        //string str = "";
        //for (int i = 0; i < recordedCommands.Count; i++)
        //{
        //    str += recordedCommands.ElementAt(i).ToString() + " ";
        //}
        //Debug.Log(str);
    }

    public void SaveReplayData()
    {
        CommandsToSerializableCommand();

        int stageIndex = GameManager.instance.stageIndex;
        string folderPath = Path.Combine(Application.persistentDataPath, "Replays");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = $"Stage_{stageIndex}_replay.json";
        string fullPath = Path.Combine(folderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log($"기존 리플레이 파일 삭제: {fullPath}");
        }

        CommandWrapper wrapper = new CommandWrapper { commands = serializableCommands };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(fullPath, json);
        Debug.Log($"리플레이 저장 완료: {fullPath}");

        serializableCommands.Clear();
        recordedCommands.Clear();
    }

    public void CommandsToSerializableCommand()
    {
        List<SerializableCommand> serializableCommands = DataManager.instance.serializableCommands;
        foreach (var kvp in DataManager.instance.recordedCommands)
        {
            SerializableCommand sc = new SerializableCommand
            {
                frame = kvp.Key,
                commandType = kvp.Value.command.ToString() // 또는 커맨드마다 이름 구분 방식 정의
            };
            serializableCommands.Add(sc);
        }
    }

    public bool LoadReplayData()
    {
        int stageIndex = GameManager.instance.stageIndex;
        string path = Path.Combine(Application.persistentDataPath, "Replays", $"Stage_{stageIndex}_replay.json");
        if (!File.Exists(path))
        {
            Debug.LogWarning("Replay Data가 없습니다: " + path);
            return false;
        }

        string json = File.ReadAllText(path);
        if (!string.IsNullOrEmpty(json))
        {
            CommandWrapper wrapper = JsonUtility.FromJson<CommandWrapper>(json);
            recordedCommands.Clear();

            foreach (var sc in wrapper.commands)
            {
                Command cmd = ConvertToCommand(sc.commandType);
                recordedCommands.Add(sc.frame, cmd);
            }
        }
        return true;
    }

    private Command ConvertToCommand(string commandType)
    {
        switch (commandType)
        {
            case "JUMP":
                return new JumpCommand();
            case "ROTATE_LEFT":
                return new RotateCommand(CommandType.ROTATE_LEFT);
            case "ROTATE_RIGHT":
                return new RotateCommand(CommandType.ROTATE_RIGHT);
            case "RECOVER_DIR":
                return new RecoverDirCommand();
            default:
                Debug.LogWarning($"Unknown command: {commandType}");
                return null;
        }
    }

    public void ClearData()
    {
        recordingFrame = 0;
        recordedCommands.Clear();
        serializableCommands.Clear();
        snapshots.Clear();
    }
}
