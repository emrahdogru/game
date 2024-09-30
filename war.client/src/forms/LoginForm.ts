import axios from 'axios'
import { useUserStore } from '@/stores/user';

export class LoginForm {
    public Email: string;
    public Password: string;

    constructor() {
        this.Email = '';
        this.Password = '';
    }

    public submit() {
        axios.post('login', this)
            .then(x => {
                const userStore = useUserStore();
                userStore.$patch(x.data);

                axios.defaults.headers['token'] = x.data.key;

                console.log(x.data);
            })
            .catch(x => {
                alert('Oturum açılamadı!');
                console.error(x);
            })
    }
}