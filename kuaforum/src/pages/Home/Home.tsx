import { Box, Container, Grid, Typography } from '@mui/material'
import { useState } from 'react';
import bgImage from "../../assets/images/pexels-cottonbro-3992855.jpg";
import ResponsiveAppBar from '../../layouts/navbar/ResponsiceAppBar';
import footerRoutes from '../../routes/footer.routes';
import { LoginForm } from '../../sections/auth/login';
import Footer from '../../sections/footer';
import Download from '../../sections/home/Download';

const Home = () => {

  // const [user,setUser] = useState(true);

  return (
    <>
    <ResponsiveAppBar/>
    <Box
        minHeight="75vh"
        width="100%"
        sx={{
          backgroundImage: `url(${bgImage})`,
          backgroundSize: "cover",
          backgroundPosition: "top",
          display: "grid",
          placeItems: "center",
        }}
      >
        <Container>
          <Grid container item xs={12} lg={7} justifyContent="center" mx="auto" flexDirection="column" alignItems="center">
            <Typography
              variant="h1"
              color="white"
              mt={-6}
              mb={1}
              sx={{ fontSize: '20px'}}
            >
              Kuaforum 
            </Typography>
            <Typography
              variant="body1"
              color="white"
              textAlign="center"
              px={{ xs: 6, lg: 12 }}
              mt={1}
            >
              Version 0.0.1 Kuaforum uygulamasına giriş sayfası 
            </Typography>
          </Grid>
        </Container>
      </Box>
        <Download />
        <Footer content={footerRoutes}/>
    </>
  )
}

export default Home