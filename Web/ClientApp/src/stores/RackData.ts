import axios from 'axios';
import { makeObservable, observable, action, computed, toJS, runInAction } from 'mobx';
import { baseURL } from '../utils/Utils';

export const urls = {
  racklocation: `${baseURL}/api/racklocation`,
  moldDataAll: `${baseURL}/api/racklocation/moldinfo`,
  machineStatus: `${baseURL}/api/racklocation/statuses`,
  location: `${baseURL}/api/racklocation/location`,
};

export default class RackData {
  //0228 seo
  RackList: any[] = [];
  RackMoldInfo: any[] = [];
  MachineStatus: any = {};
  MyRackPosition: any = '';
  MyRackFloor: string = '';
  MachineLocationInfo: any = {};
  ClickedTrigger: boolean = false;

  constructor() {
    makeObservable(this, {
      RackList: observable,
      RackMoldInfo: observable,
      MachineStatus: observable,
      MyRackPosition: observable,
      MyRackFloor: observable,
      MachineLocationInfo: observable,
      ClickedTrigger: observable,

      downRackListData: action,
      downRackMoldData: action,
      downMachineStatus: action,
      setMyRackPosition: action,
      setMyRackFloor: action,
      downMachineLocationInfo: action,

      getRackList: computed,
      getRackMoldInfo: computed,
      getMachineStatus: computed,
      getMyRackPosition: computed,
      getMyRackFloor: computed,
      getMachineLocationInfo: computed,
      getClickedTrigger: computed,
    });
  }

  downRackListData = async () => {
    const response = await axios.get(urls.racklocation);
    // console.log('response.data', response.data);

    return runInAction(() => {
      this.RackList = response.data;
    });
  };

  downRackMoldData = async () => {
    try {
      const response = await axios.get(urls.moldDataAll);
      return runInAction(() => {
        this.RackMoldInfo = response.data;
      });
    } catch (err) {
      console.log(err);
      return runInAction(() => {
        this.RackMoldInfo = [];
      });
    }
  };
  downMachineStatus = async () => {
    const response = await axios.get(urls.machineStatus);
    return runInAction(() => {
      this.MachineStatus = response.data;
    });
  };

  setMyRackPosition = (data: string | boolean) => {
    return runInAction(() => {
      this.MyRackPosition = data;
      this.ClickedTrigger = !this.ClickedTrigger;
    });
  };

  setMyRackFloor = (data: string) => {
    return runInAction(() => {
      this.MyRackFloor = data;
    });
  };

  downMachineLocationInfo = async () => {
    const response = await axios.get(urls.location);
    return runInAction(() => {
      this.MachineLocationInfo = response.data;
    });
  };
  get getRackList() {
    return this.RackList;
  }
  get getRackMoldInfo() {
    return this.RackMoldInfo;
  }

  get getMachineStatus() {
    return this.MachineStatus;
  }

  get getMyRackPosition() {
    return this.MyRackPosition;
  }
  get getClickedTrigger() {
    return this.ClickedTrigger;
  }
  get getMyRackFloor() {
    return this.MyRackFloor;
  }

  get getMachineLocationInfo() {
    return this.MachineLocationInfo;
  }
}
