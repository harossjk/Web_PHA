import axios from 'axios';
import { makeObservable, observable, action, computed, toJS } from 'mobx';
import { configure } from 'mobx';

const baseURL = `http://localhost:4000`;

export const urls = {
  user: `${baseURL}/users`,
  companyList: `${baseURL}/companyList`,
};

export default class UserStore {
  usersData: any[] = [];

  companysDatat: any[] = [];

  constructor() {
    makeObservable(this, {
      usersData: observable,
      companysDatat: observable,

      downUser: action,
      downCompanyList: action,

      getUsersData: computed,
      getCompanysData: computed,
    });
  }

  initStart = async () => {
    await this.downUser();
    await this.downCompanyList();
  };

  downUser = async () => {
    try {
      const response = await axios.get(urls.user);
      console.log('axios에서 가져온 데이터', response.data);
      this.usersData = response.data;
    } catch (err) {
      console.log(err);
    }
  };

  downCompanyList = async () => {
    try {
      const response = await axios.get(urls.companyList);
      console.log('axios에서 가져온 데이터', response.data);
      this.companysDatat = response.data;
    } catch (err) {
      console.log(err);
    }
  };

  get getUsersData() {
    return this.usersData;
    // return this.usersData.length > 0 ? this.usersData : InitialData;
  }

  get getCompanysData() {
    return this.companysDatat;
    // return this.usersData.length > 0 ? this.usersData : InitialData;
  }
}
