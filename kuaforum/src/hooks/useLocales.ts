import { useTranslation } from 'react-i18next';
// @mui
import { enUS, deDE, frFR, trTR } from '@mui/material/locale';
import axios from '../utils/axios';

// ----------------------------------------------------------------------

const LANGS = [
  {
    label: 'Türkçe',
    value: 'TR',
    systemValue: trTR,
    icon: '/icons/ic_tr_flag.svg',
  },
  {
    label: 'English',
    value: 'en-US',
    systemValue: enUS,
    icon: 'https://minimal-assets-api.vercel.app/assets/icons/ic_flag_en.svg',
  },
  {
    label: 'German',
    value: 'DE',
    systemValue: deDE,
    icon: 'https://minimal-assets-api.vercel.app/assets/icons/ic_flag_de.svg',
  },
  {
    label: 'French',
    value: 'FR',
    systemValue: frFR,
    icon: 'https://minimal-assets-api.vercel.app/assets/icons/ic_flag_fr.svg',
  },
];

export default function useLocales() {
  const { i18n, t: translate } = useTranslation();
  const langStorage = localStorage.getItem('i18nextLng');
  const currentLang = LANGS.find((_lang) => _lang.value === langStorage) || LANGS[1];
  
  const handleChangeLanguage = async(newlang: string) => {
    const resLang = await axios.get(`/Translate/${newlang}`)
    i18n.changeLanguage(newlang);
  };

  return {
    onChangeLang: handleChangeLanguage,
    translate,
    currentLang,
    allLang: LANGS,
  };
}
